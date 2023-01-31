import React, { useState } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Button, Text, TextInput, Card, ToggleButton, Checkbox } from 'react-native-paper';
import { Formik, FieldArray, ErrorMessage } from 'formik';
import { THEME } from '../../theme';
import * as Yup from 'yup';

const initialForm = {
    question: "",
    answers: ["Потому что ёж", "Капибара"],
    rightAnswer: []
}
const singleValidation = Yup.object().shape({
    question: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    answers: Yup.array().of(Yup.string().required('Поле обязательно')),
    rightAnswer: Yup.array()
        .min(1, 'Укажите верные ответы'),
});


const AddSingleQuestion = ({ route, navigation }) => {

    const submitHandler = (values) => {
        route.params.onReturn({ ...values, type: "multiple" });
        navigation.goBack()
    }
    const initialForm = route.params?.question || {
        question: "",
        answers: ["Потому что ёж", "Капибара"],
        rightAnswer: []
    }

    const setChecked = (values, item) => {
        let result = [...values.rightAnswer]
        let index = result.indexOf(item)
        if (index != -1) {
            result.splice(index, 1)
        } else {
            result = [...new Set([...result, item])]
        }
        return result
    }

    return (
        <>
            <Formik
                initialValues={initialForm}
                validationSchema={singleValidation}
                onSubmit={values => submitHandler(values)}
            >
                {({ handleChange, handleBlur, handleSubmit, values, setValues, setFieldValue, errors, touched }) => (
                    <ScrollView >
                        <View style={styles.mainForm}>
                            <Text style={styles.title} variant="titleMedium">Вопрос с мультивыбором</Text>
                            <TextInput
                                error={touched.question && errors.question ? errors.question : ""}
                                label="Вопрос"
                                mode="outlined"
                                style={styles.input}
                                onChangeText={handleChange('question')}
                                onBlur={handleBlur('question')}
                                value={values.question}
                            />
                            <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                <ErrorMessage name="question" />
                            </Text>
                            <Card style={styles.card}>
                                <Card.Content >
                                    <Text style={styles.title} variant="titleMedium">Варианты ответа</Text>
                                    <FieldArray
                                        name="answers"
                                        render={arrayHelpers => (<>
                                            {values.answers.map((item, index) => (

                                                <>
                                                    <TextInput
                                                        style={styles.input}
                                                        error={
                                                            touched.hasOwnProperty('answers')
                                                            && errors.hasOwnProperty('answers')
                                                            && touched.answers[index]
                                                            && errors.answers[index]
                                                        }
                                                        label={`Ответ ${index + 1}`}
                                                        mode="outlined"
                                                        onChangeText={e => setFieldValue(`answers.${index}`, e)}
                                                        value={values.answers[index]}
                                                        right={<TextInput.Icon
                                                            onPress={() => values.answers.length > 2 ? arrayHelpers.remove(index) : ""}
                                                            color={() => THEME.colors.secondary} style={styles.iconDelete} icon="delete" />}
                                                    />
                                                    <Checkbox.Item
                                                        labelVariant="labelSmall"
                                                        position='leading'
                                                        style={{ height: 30, marginTop:4 }}
                                                        status={values.rightAnswer.indexOf(item) !== -1 ? "checked" : "unchecked"}
                                                        onPress={() => setFieldValue("rightAnswer", setChecked(values, item))}
                                                        label="Отметить верным" />
                                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                                        <ErrorMessage name={`answers.[${index}]`} />
                                                    </Text>
                                                </>
                                            ))}
                                            <Button style={styles.button}
                                                onPress={() => arrayHelpers.push("")}
                                                icon="plus" mode="contained" title="Submit"
                                            >
                                                Вариант
                                            </Button>
                                            <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                                <ErrorMessage name="rightAnswer" />
                                            </Text>
                                        </>)}
                                    ></FieldArray>
                                </Card.Content>
                            </Card>
                            <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Сохранить</Button>
                        </View>

                    </ScrollView>
                )}
            </Formik>

        </>
    );
}

const styles = StyleSheet.create({
    mainForm: {
        padding: 8,
        paddingTop: 10
    },
    button: {
        marginTop: 2,
        marginBottom: 6
    },
    input: {
        marginBottom: 0
    },
    card: {
        borderRadius: 1,
        marginTop: 10,
        marginBottom: 10
    },
    iconDelete: {
        color: THEME.colors.error
    },
    title: {
        marginBottom: 8
    }
})

export default AddSingleQuestion;
