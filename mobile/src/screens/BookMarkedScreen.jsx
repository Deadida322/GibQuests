import React, { useState, useEffect } from 'react';
import { View, StyleSheet, Text } from 'react-native';
import { TextInput, Button, Banner, Portal, Dialog, Snackbar } from 'react-native-paper';
import { THEME } from "../theme"
import FABMenu from '../components/FABMenu';
import StageItem from '../components/StageItem';
import Ionicons from '@expo/vector-icons/Ionicons';
import { Formik } from 'formik';
import { ScrollView } from 'react-native-gesture-handler';
import ImageLoader from '../components/UI/ImageLoader';
import store, { currentQuest } from '../store/createQuest';
import { observer, Observer } from "mobx-react"
import request, { axiosInstance } from '../request';
import auth from '../store/auth';

export default observer(function BookMarkedScreen({ navigation }) {
    useEffect(() => {
        if (store.currentQuest.id) {
            store.get()
        }
    }, [])
    useEffect(() => {
        if (store.currentQuest.id) {
            store.get()
            console.log("get")

        }
        setImage(store.currentQuest.img)
    }, [store.currentQuest.img])

    const [image, setImage] = useState("")
    const [showSnack, setShowSnack] = useState(false)
    const [showFAB, setShowFab] = useState(false);
    const [currentDeletable, setCurrentDeletable] = useState(false)

    function openStage(type) {
        switch (type) {
            case "text": {
                navigation.navigate('AddText', {
                    onReturn: (item) => {
                        store.setCurrentEditable({ ...store.currentQuest, stages: [...store.currentQuest.stages, item] })
                    }
                })
                break
            }
            case "video": {
                navigation.navigate('AddVideo', {
                    onReturn: (item) => {
                        store.setCurrentEditable({ ...store.currentQuest, stages: [...store.currentQuest.stages, item] })
                    }
                })
                break
            }
            case "qrcode": {
                navigation.navigate('AddQR', {
                    onReturn: (item) => {
                        store.setCurrentEditable({ ...store.currentQuest, stages: [...store.currentQuest.stages, item] })
                    }
                })
                break
            }
            case "map": {
                navigation.navigate('AddMap', {
                    onReturn: (item) => {
                        store.setCurrentEditable({ ...store.currentQuest, stages: [...store.currentQuest.stages, item] })
                    }
                })
                break
            }
            case "test": {
                navigation.navigate('AddTest', {
                    onReturn: (item) => {
                        store.setCurrentEditable({ ...store.currentQuest, stages: [...store.currentQuest.stages, item] })
                    },
                    item: "aa"
                })
                break
            }
        }

    }

    const clearAll = (values) => {
        values.title = ""
        values.description = ""
        store.clearCurrentEditable()
        setImage("")
    }


    const onStageEdit = (stage, index) => {
        const navigating = (path, index) => {
            navigation.navigate(path, {
                onReturn: (item) => {
                    console.log(item)
                    let newItems = [...store.currentQuest.stages]
                    newItems.splice(index, 1, item)
                    store.setCurrentEditable({ ...store.currentQuest, stages: newItems })
                },
                stage
            })
        }
        switch (stage.type) {
            case "text": {
                navigating("AddText")
                break
            }
            case "video": {
                navigating("AddVideo")
                break
            }
            case "qrcode": {
                navigating("AddQR")
                break
            }
            case "map": {
                navigating("AddMap")
                break
            }
            case "test": {
                navigating("AddTest")
                break
            }
        }
    }

    const onStageDelete = (index) => {
        let newItems = [...store.currentQuest.stages]
        newItems.splice(index, 1)
        store.setCurrentEditable({ ...store.currentQuest, stages: newItems })
        setCurrentDeletable(false)
    }

    const submitForm = (quest) => {
        if (store.currentQuest?.stages?.length > 1) {
            let data = { ...store.currentQuest, ...quest, img: image, stages: [...store.currentQuest.stages].map((val, index) => ({ ...val, order: index })), policy: {policyType: "public", memberType: "group" } }
            let method = "POST"
            let url = "/GenerateQuest/CreateQuest"
            if (store.currentQuest.id) {
                method = "PUT"
                url = "/GenerateQuest/UpdateQuest"
            }
            console.log(data, "payload")
            request({
                url,
                method,
                data,
                headers: { 'Authorization': `Bearer ${auth.accessToken}` },

            })
                .then((res) => {
                    console.log(res, "submitForm")
                })
                .catch((err) => {
                    console.log(err, "error")
                });
        } else {
            setShowSnack(true)
        }
    }

    return (
        <>
            <Snackbar
                visible={showSnack}
                style={{ zIndex: 9999 }}
                onDismiss={() => setShowSnack(false)}
                action={{
                    label: 'Ок',
                    onPress: () => {
                        setShowSnack(false)
                    },
                }}>
                Добавьте от двух этапов
            </Snackbar>
            <Portal>
                <Dialog style={{ borderRadius: 4 }} visible={currentDeletable} onDismiss={() => setCurrentDeletable(false)}>
                    <Dialog.Title>Вы уверены?</Dialog.Title>
                    <Dialog.Content>
                        {currentDeletable ? <>
                            <Text variant="bodyMedium">Вы действительно хотите удалить этап: </Text>
                            <Text style={{ color: THEME.colors.primary }} variant='bodyLarge'>{currentDeletable.title}</Text>
                        </> : ""}
                    </Dialog.Content>
                    <Dialog.Actions>
                        <Button onPress={() => setCurrentDeletable(false)}>Отмена</Button>
                        <Button onPress={() => onStageDelete(currentDeletable.index)}>Да</Button>
                    </Dialog.Actions>
                </Dialog>
            </Portal>
            <ScrollView >
                <View style={styles.mainForm}>
                    <Formik
                        initialValues={{ ...store.currentQuest }}
                        onSubmit={values => submitForm(values)}
                    >
                        {({ handleChange, handleBlur, handleSubmit, values, resetForm }) => (
                            <Observer>{() => (

                                <View>
                                    <Text style={styles.title} variant="titleMedium">{values.title || 'Название квеста'}</Text>
                                    <ImageLoader img={image} onImageLoad={(e) => setImage(e)}></ImageLoader>
                                    <TextInput
                                        label="Название"
                                        mode="outlined"
                                        onChangeText={handleChange('title')}
                                        onBlur={handleBlur('title')}
                                        value={values.title}
                                    />

                                    <TextInput
                                        label="Описание"
                                        mode="outlined"
                                        numberOfLines={4}
                                        multiline={true}
                                        style={styles.input}
                                        onChangeText={handleChange('description')}
                                        onBlur={handleBlur('description')}
                                        value={values.description}
                                    />
                                    <View style={styles.buttonContainer}>
                                        <Button mode="contained-tonal" onPress={() => clearAll(values)}>Сбросить</Button>
                                        <Button onPress={() => handleSubmit()} mode="contained" style={{ marginLeft: 6 }}>Сохранить</Button>
                                    </View>
                                </View>
                            )}</Observer>
                        )}
                    </Formik>


                    <Text style={styles.title} variant="titleMedium">Этапы</Text>
                    {store.currentQuest.stages?.length ? store.currentQuest.stages.map((item, index) => (
                        <StageItem
                            key={item.title}
                            stage={item}
                            onStageDelete={() => setCurrentDeletable({ ...item, index })}
                            onStageEdit={() => onStageEdit(item, index)} />
                    )) : ""}
                    {(store.currentQuest.stages?.length < 2 || !store.currentQuest.stages) ?
                        <Banner
                            visible={true}
                            actions={[
                                {
                                    label: 'Добавить',
                                    onPress: () => setShowFab(true)
                                }
                            ]}
                            icon={() => (
                                <Ionicons name="alert-circle-outline" size={32} color="blue" />
                            )}
                        >
                            Необходимо добавить хотя бы несколько этапов для вашего квеста
                        </Banner> : ""}
                </View>
            </ScrollView>
            <FABMenu
                onStateChange={(a) => setShowFab(a.open)}
                show={showFAB}
                onOpenStage={openStage}
            />
        </>
    );
})

const styles = StyleSheet.create({
    mainForm: {
        padding: 8,
        paddingTop: 10,
        paddingBottom: 80
    },
    input: {
        marginTop: 10
    },
    title: {
        color: THEME.PRIMARY_COLOR,
        marginBottom: 4,
        marginTop: 6
    },
    fab: {
        position: "absolute",
        margin: 16,
        right: 0,
        bottom: 0,
    },
    buttonContainer: {
        flexDirection: "row",
        justifyContent: 'flex-end',
        marginTop: 6
    }

})

