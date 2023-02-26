import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { StyleSheet, ActivityIndicator, View, ScrollView, Dimensions, BackHandler } from "react-native";
import { THEME } from "../theme"
import { FAB, Button, Text, Card, Searchbar } from "react-native-paper";
import QuestItem from "../components/QuestItem";
import { setCurrentQuest } from "../store/actions/quest";

const completed = [{
    title: "Найди меня",
    description: "lorem ipsum dolor sit amet sab grou derumo matis, silico der toilo potoroto yomokato",
    img: "https://avatars.mds.yandex.net/i?id=00bbea24f4ab1b3637add8fc4e244d57cb045c21-7674757-images-thumbs&n=13",

    stages: [
        {
            title: "test",
            type: "test",
            questions: [
                {
                    title: "Сколько хош",
                    type: "insert",
                    answers: [
                        "адин",
                    ],
                    rightAnswer: [
                        "адин",

                    ]
                },
                {
                    title: "Сколько хош",
                    type: "single",
                    answers: [
                        "адин",
                        "два",
                        "три"
                    ],
                    rightAnswer: [
                        "адин"
                    ]
                },
                {
                    title: "Сколько хош",
                    type: "multiple",
                    answers: [
                        "адын",
                        "Два",
                        "Три"
                    ],
                    rightAnswer: [
                        "адин",
                        "Два"
                    ]
                },
                {
                    title: "Сколько хош",
                    type: "order",
                    answers: [
                        "Четыре",
                        "Два",
                        "Три"
                    ],
                    rightAnswer: [
                        "Четыре",
                        "Два",
                        "Три"
                    ]
                },
            ]
        },
        {
            title: "qr",
            type: "qrcode",
            code: "asdasdsda"
        },
        {
            title: "Youtube",
            type: "video",
            url: "asdasdsda"
        },
        {
            title: "Первый этап",
            type: "text",
            description: "asdasdsda"
        },
        {
            title: "Найди дорогу домой",
            type: "map",
            description: "asdasdsda",
            coords: {
                longitude: 48.3971502,
                latitude: 54.3438362
            }
        },
        {
            title: "СНова текст",
            type: "text",
            description: "LoremDolar sit amet"
        },
        {
            title: "Просмотри",
            type: "video",
            url: "asdasdsda"
        },
    ]
}]

export default function MainScreen({ navigation, route }) {
    const dispatch = useDispatch()
    const onWatch = () => {
        navigation.navigate("WatchScreen")
    }
    const onEdit = () => {
        dispatch(setCurrentQuest(completed[0]))
        navigation.navigate("Add")
    }

    const onBackPress = () => {
        console.log(route.name)
        if (route.name === 'Main') {
            return true;
        } else {
            return false;
        }
    };
    
    useEffect(() => {
        navigation.addListener('focus', () => {
            BackHandler.addEventListener('hardwareBackPress', onBackPress)
        })
        navigation.addListener('blur', () => {
            BackHandler.removeEventListener('hardwareBackPress', onBackPress)
        })
    }, [])


    return (
        <ScrollView>
            <View style={styles.wrapper}>
                <Text style={styles.title} variant="titleMedium">Создано вами</Text>
                <Searchbar
                    style={styles.search}
                    placeholder="Искать среди своих"
                // onChangeText={onChangeSearch}
                // value={searchQuery}
                />
                <ScrollView style={{ marginTop: 10 }} horizontal={true}>
                    {[...completed, ...completed].map(item => (
                        <View style={{ width: Dimensions.get("screen").width - 48, margin: 3, marginBottom: 10, marginRight: 8 }}>
                            <QuestItem
                                isAdmin={true}
                                onEdit={()=>onEdit()}
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
                // onChangeText={onChangeSearch}
                // value={searchQuery}
                />
                <ScrollView style={{ marginTop: 10 }} horizontal={true}>
                    {[...completed, ...completed].map(item => (
                        <View style={{ width: Dimensions.get("screen").width - 48, margin: 3, marginBottom: 10, marginRight: 8 }}>
                            <QuestItem onQuestPreview={() => onQuestPreview(item)} quest={item} />
                        </View>
                    )
                    )}
                </ScrollView>
            </View>
        </ScrollView>
    );
}

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
