import React, { useState } from 'react';
import { View, StyleSheet, FlatList } from 'react-native';
import { TextInput, Button, Text, Banner, Portal, Dialog } from 'react-native-paper';
import { useSelector } from 'react-redux';
import { THEME } from "../theme"
import FABMenu from '../components/FABMenu';
import StageItem from '../components/StageItem';
import Ionicons from '@expo/vector-icons/Ionicons';
import { Formik } from 'formik';
import { ScrollView } from 'react-native-gesture-handler';

const BookMarkedScreen = ({ route, navigation }) => {
    const [showFAB, setShowFab] = useState(false);
    const [stages, setStages] = useState([]);
    const [currentDeletable, setCurrentDeletable] = useState(false)

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


    const onStageEdit = (stage, index) => {
        switch (stage.type) {
            case "text": {
                navigation.navigate('AddText', {
                    onReturn: (item) => {
                        let newItems = [...stages]
                        newItems.splice(index, 1, item)
                        setStages([...newItems])
                    },
                    stage
                })
                break
            }
            case "video": {
                navigation.navigate('AddVideo', {
                    onReturn: (item) => {
                        let newItems = [...stages]
                        newItems.splice(index, 1, item)
                        setStages([...newItems])
                    },
                    stage
                })
                break
            }
            case "qrcode": {
                navigation.navigate('AddQR', {
                    onReturn: (item) => {
                        let newItems = [...stages]
                        newItems.splice(index, 1, item)
                        setStages([...newItems])
                    },
                    stage
                })
                break
            }
            case "map": {
                navigation.navigate('AddMap', {
                    onReturn: (item) => {
                        let newItems = [...stages]
                        newItems.splice(index, 1, item)
                        setStages([...newItems])
                    },
                    stage
                })
                break
            }
            case "test": {
                navigation.navigate('AddTest', {
                    onReturn: (item) => {
                        let newItems = [...stages]
                        newItems.splice(index, 1, item)
                        setStages([...newItems])
                    },
                    stage
                })
                break
            }
        }
    }

    const onStageDelete = (index) => {
        let newItems = [...stages]
        newItems.splice(index, 1)
        setStages([...newItems])
        setCurrentDeletable(false)
    }

    return (
        <>
            <Portal>
                <Dialog style={{borderRadius:4}} visible={currentDeletable} onDismiss={()=>setCurrentDeletable(false)}>
                    <Dialog.Title>Вы уверены?</Dialog.Title>
                    <Dialog.Content>
                       {currentDeletable ? <>
                            <Text variant="bodyMedium">Вы действительно хотите удалить этап: </Text>
                            <Text style={{ color: THEME.colors.primary }} variant='bodyLarge'>{currentDeletable.title}</Text>
                        </>: ""}
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

                            <Text style={styles.title} variant="titleMedium">Этапы</Text>
                            {stages.map((item, index) => (
                                <StageItem stage={item} onStageDelete={() => setCurrentDeletable({ ...item, index })} onStageEdit={() => onStageEdit(item, index)}></StageItem>
                            ))}
                            {(stages.length < 2) ? <Banner
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
                            <Button onPress={handleSubmit} title="Submit" />
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
        paddingTop: 10
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
})

export default BookMarkedScreen;
