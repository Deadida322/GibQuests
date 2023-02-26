import React, { useState, useEffect } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Button, Text, TextInput, Snackbar } from 'react-native-paper';
import NoQuestions from '../../components/AddTest/NoQuestions';
import AddTestFAB from "../../components/AddTest/AddTestFAB"
import { Formik, ErrorMessage } from 'formik';
import QuestItem from '../../components/AddTest/QuestItem';
import * as Yup from 'yup';
import { THEME } from '../../theme';



const validation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
});

const AddTestMain = ({ route, navigation }) => {
    const params = navigation.getParent().getState().routes.find(i => i.name == "AddTest").params

    const [showFAB, setShowFab] = useState(false);
    const [showSnack, setShowSnack] = useState(false);
    const initialForm = params?.stage || {
        title: "",
        type: "test",
        questions: []
    }
    const [questions, setQuestions] = useState(initialForm.questions)

    const submitHandler = (values) => {
        if (!questions.length) {
            setShowSnack(true)
            return
        }
        params.onReturn(
            {...values, questions}
        );
        navigation.goBack()
    }



    const deleteQuestion = (index) => {
        let newItems = [...questions]
        newItems.splice(index, 1)
        setQuestions([...newItems])
    }

    const editQuestion = (question, index) => {
        switch (question.type) {
            case "single": {
                navigation.navigate('AddSingleQuestion', {
                    onReturn: (item) => {
                        let newItems = [...questions]
                        newItems.splice(index, 1, item)
                        setQuestions([...newItems])
                    },
                    question
                })
                break;
            }
            case "multiple": {
                navigation.navigate('AddMutlipleQuestion', {
                    onReturn: (item) => {
                        let newItems = [...questions]
                        newItems.splice(index, 1, item)
                        setQuestions([...newItems])
                    },
                    question
                })
                break;

            }
            case "insert": {
                navigation.navigate('AddInsertQuestion', {
                    onReturn: (item) => {
                        let newItems = [...questions]
                        newItems.splice(index, 1, item)
                        setQuestions([...newItems])
                    },
                    question
                })
                break;

            }
            case "order": {
                navigation.navigate('AddOrderQuestion', {
                    onReturn: (item) => {
                        setQuestions(prev => [...prev, item])
                    },
                    question
                })
                break;

            }
        }
    }
    const openQuestion = (type) => {
        switch (type) {
            case "single": {
                navigation.navigate('AddSingleQuestion', {
                    onReturn: (item) => {
                        setQuestions(prev => [...prev, item])
                    }
                })
                break;
            }
            case "multiple": {
                navigation.navigate('AddMutlipleQuestion', {
                    onReturn: (item) => {
                        setQuestions(prev => [...prev, item])
                    }
                })
                break;

            }
            case "insert": {
                navigation.navigate('AddInsertQuestion', {
                    onReturn: (item) => {
                        setQuestions(prev => [...prev, item])
                    }
                })
                break;

            }
            case "order": {
                navigation.navigate('AddOrderQuestion', {
                    onReturn: (item) => {
                        setQuestions(prev => [...prev, item])
                    }
                })
                break;

            }
        }
    }
    return (
        <>
            <Formik
                initialValues={initialForm}
                onSubmit={values => submitHandler(values)}
                validationSchema={validation}
            >
                {({ handleChange, handleBlur, handleSubmit, values, touched, errors }) => (
                    <View style={styles.mainForm}>
                        <Text style={styles.title} variant="titleMedium">Создание тестового этапа</Text>
                        <TextInput
                            label="Название"
                            mode="outlined"
                            error={touched.title && errors.title ? errors.title : ""}
                            onChangeText={handleChange('title')}
                            onBlur={handleBlur('title')}
                            value={values.title}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="title" />
                        </Text>
                        <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Добавить</Button>
                        <Text style={styles.title} variant="titleMedium">Вопросы:</Text>
                        <ScrollView style={styles.questionContainer}>

                            {questions.length ? questions.map((item, key) => (
                                <QuestItem onQuestionDelete={() => deleteQuestion(key)} onQuestionEdit={() => editQuestion(item, key)} index={key + 1} question={item}></QuestItem>
                            )) : <NoQuestions></NoQuestions>}
                        </ScrollView>

                    </View>
                )}
            </Formik>
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
                Добавьте вопросы
            </Snackbar>
            <AddTestFAB
                onStateChange={(a) => setShowFab(a.open)}
                show={showFAB}
                onOpenQuestion={openQuestion}
            />
        </>
    );
}

const styles = StyleSheet.create({
    mainForm: {
        padding: 8,
        paddingTop: 10
    },
    button: {
        marginTop: 6,
        marginBottom: 6
    },
    input: {
        marginTop: 8,
        marginBottom: 8
    },
    questionContainer: {
        marginTop: 10,
        marginBottom: 160
    }
})

export default AddTestMain;
