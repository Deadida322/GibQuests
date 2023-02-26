import React, { useState, useEffect } from 'react';
import { View, StyleSheet, ImageBackground } from 'react-native';
import { useDispatch, useSelector } from 'react-redux';
import { TextInput, Button, Text, Banner, Portal, Dialog } from 'react-native-paper';
import { THEME } from "../theme"
import FABMenu from '../components/FABMenu';
import StageItem from '../components/StageItem';
import Ionicons from '@expo/vector-icons/Ionicons';
import { Formik } from 'formik';
import { ScrollView } from 'react-native-gesture-handler';
import * as ImagePicker from 'expo-image-picker';
import ImageLoader from '../components/UI/ImageLoader';
import { questReducer } from '../store/reducers/quest';
import { setCurrentQuest } from '../store/actions/quest';

const BookMarkedScreen = ({ route, navigation }) => {

    const [stages, setStages] = useState([]);
    const [image, setImage] = useState("")
    const [showFAB, setShowFab] = useState(false);
    const [currentDeletable, setCurrentDeletable] = useState(false)
    const dispatch = useDispatch()
    let currentQuest = useSelector((state) => (state.quest.currenEditableQuest))
    useEffect(() => {
        if (currentQuest) {
            setStages(currentQuest.stages)
        }
        console.log(currentQuest)
    })

    function openStage(type) {
        switch (type) {
            case "text": {
                navigation.navigate('AddText', {
                    onReturn: (item) => {
                        setStages(prev => [...prev, item])
                    }
                })
                break
            }
            case "video": {
                navigation.navigate('AddVideo', {
                    onReturn: (item) => {
                        setStages(prev => [...prev, item])
                    }
                })
                break
            }
            case "qrcode": {
                navigation.navigate('AddQR', {
                    onReturn: (item) => {
                        setStages(prev => [...prev, item])
                    }
                })
                break
            }
            case "map": {
                navigation.navigate('AddMap', {
                    onReturn: (item) => {
                        setStages(prev => [...prev, item])
                    }
                })
                break
            }
            case "test": {
                navigation.navigate('AddTest', {
                    onReturn: (item) => {
                        setStages(prev => [...prev, item])
                    },
                    item: "aa"
                })
                break
            }
        }

    }

    const clearAll = () => {
        dispatch(setCurrentQuest({}))
    }


    const onStageEdit = (stage, index) => {
        const navigating = (path, index) => {
            navigation.navigate(path, {
                onReturn: (item) => {
                    let newItems = [...stages]
                    newItems.splice(index, 1, item)
                    dispatch(setCurrentQuest({ ...currentQuest, stages: newItems }))
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
        let newItems = [...stages]
        newItems.splice(index, 1)
        dispatch(setCurrentQuest({ ...currentQuest, stages: newItems }))
        setCurrentDeletable(false)
    }

    return (
        <>
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
            <ScrollView>
                <Formik
                    initialValues={{ title: "", description: '' }}
                    onSubmit={values => console.log(values)}
                >
                    {({ handleChange, handleBlur, handleSubmit, values }) => (
                        <View style={styles.mainForm}>
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
                                <Button mode="contained-tonal" onPress={() => clearAll()}>Сбросить</Button>
                                <Button mode="contained" style={{ marginLeft: 6 }}>Сохранить</Button>
                            </View>
                            <Text style={styles.title} variant="titleMedium">Этапы</Text>
                            {stages?.length ? stages.map((item, index) => (
                                <StageItem stage={item} onStageDelete={() => setCurrentDeletable({ ...item, index })} onStageEdit={() => onStageEdit(item, index)}></StageItem>
                            )) : ""}
                            {(stages?.length < 2 || !stages) ? <Banner
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
                    )}
                </Formik>
            </ScrollView>
            <FABMenu
                onStateChange={(a) => setShowFab(a.open)}
                show={showFAB}
                onOpenStage={openStage}
            />
        </>
    );
}

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

export default BookMarkedScreen;
