import React, { useEffect, useState } from "react";
import { StyleSheet, View, ScrollView, Dimensions, BackHandler } from "react-native";
import { THEME } from "../theme"
import { FAB, Button, Text, Card, Searchbar, Portal, Dialog } from "react-native-paper";
import QuestItem from "../components/QuestItem";
import questStore from "../store/createQuest";
import auth from "../store/auth"
import { observer } from "mobx-react"
import request from "../request";


export default observer(function MainScreen({ navigation, route }) {
    const [createdQuests, setCreatedQuests] = useState([])
    const [processedQuests, setProcessedQuests] = useState([])
    const [currentDeletable, setCurrentDeletable] = useState(false)
    const fetchCreated = () =>{
        let data = {}
        if(auth.user.id){
            data.id = auth.user.id
        }
        request({
            url: "/GenerateQuest/GetFilteredQuests",
            method: "POST",
            headers: {'Authorization': `Bearer ${ auth.accessToken}`},
            data: data
        }).then(res=>{
            setCreatedQuests(res)
        }).catch(err=>{
            console.log(err, "err")
        })
    }
    const fetchProcessed = () =>{
        let data = {}
        if(auth.user.id){
            data.id = auth.user.id
        }
        request({
            url: "/GenerateQuest/GetProcessedQuests",
            method: "POST",
            headers: {'Authorization': `Bearer ${ auth.accessToken}`},
            data: data
        }).then(res=>{
            setProcessedQuests(res)
        }).catch(err=>{
            console.log(err, "err")
        })
    }
    const onQuestDelete = (id) =>{
        request({
            url: `/GenerateQuest/DeleteQuest/${id}`,
            method: "DELETE"
        }).then(res=>{
            setCurrentDeletable(false)
            fetchCreated()
        })
    }
    const onDelete = (item) => {
        setCurrentDeletable(item)
    }
    const onWatch = () => {
        navigation.navigate("WatchScreen")
    }
    const onEdit = (item) => {
        questStore.setCurrentEditable(item)
        navigation.navigate("UpdateScreen", { goToEdit: true})
    }

    const onBackPress = () => {
        if (route.name === 'Main') {
            return true;
        } else {
            return false;
        }
    };
    const onQuestPreview = (item) =>{
        navigation.navigate("PreviewScreen", { quest:item})
    }

    useEffect(() => {
        navigation.addListener('focus', () => {
            BackHandler.addEventListener('hardwareBackPress', onBackPress)
            fetchCreated()
            fetchProcessed()
        })
        navigation.addListener('blur', () => {
            BackHandler.removeEventListener('hardwareBackPress', onBackPress)
        })
    }, [])


    return (
        <ScrollView>
            <Portal>
                <Dialog style={{ borderRadius: 4 }} visible={currentDeletable} onDismiss={() => setCurrentDeletable(false)}>
                    <Dialog.Title>Вы уверены?</Dialog.Title>
                    <Dialog.Content>
                        {currentDeletable ? <>
                            <Text variant="bodyMedium">Вы действительно хотите удалить квест: </Text>
                            <Text style={{ color: THEME.colors.primary }} variant='bodyLarge'>{currentDeletable.title}</Text>
                        </> : ""}
                    </Dialog.Content>
                    <Dialog.Actions>
                        <Button onPress={() => setCurrentDeletable(false)}>Отмена</Button>
                        <Button onPress={() => onQuestDelete(currentDeletable.id)}>Да</Button>
                    </Dialog.Actions>
                </Dialog>
            </Portal>
            <View style={styles.wrapper}>
                <Text style={styles.title} variant="titleMedium">Создано вами</Text>
                <Searchbar
                    style={styles.search}
                    placeholder="Искать среди своих"
                />
                <ScrollView style={{ marginTop: 10 }} horizontal={true}>
                    {createdQuests.map(item => (
                        <View style={{ width: Dimensions.get("screen").width - 48, margin: 3, marginBottom: 10, marginRight: 8 }}>
                            <QuestItem
                                key={item.id}
                                isAdmin={true}
                                onDelete={() => onDelete(item)}
                                onEdit={() => onEdit(item)}
                                onQuestPreview={() => onQuestPreview(item)}
                                onWatch={() => onWatch()}
                                quest={item}
                            />
                        </View>
                    )
                    )}
                </ScrollView>

                <Text style={{ ...styles.title, marginTop: 20 }} variant="titleMedium">Пройденные / В процессе</Text>
                <Searchbar
                    elevation={1}
                    style={styles.search}
                    placeholder="Искать среди пройденных"
                />
                <ScrollView style={{ marginTop: 10 }} horizontal={true}>
                    {processedQuests.map(item => (
                        <View style={{ width: Dimensions.get("screen").width - 48, margin: 3, marginBottom: 10, marginRight: 8 }}>
                            <QuestItem onQuestPreview={() => onQuestPreview(item)} quest={item} />
                        </View>
                    )
                    )}
                </ScrollView>
            </View>
        </ScrollView>
    );
})

const styles = StyleSheet.create({
    search: {
        height: 40,
        marginVertical: 4
    },
    wrapper: {
        marginHorizontal: 10,
        marginVertical: 6
    },
    center: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
    },
    title: {
        color: THEME.PRIMARY_COLOR,
        marginBottom: 6,
        marginTop: 6
    },
});
