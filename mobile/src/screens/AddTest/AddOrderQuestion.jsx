import React, { useState, useMemo, useRef } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Button, Text, TextInput, Card } from 'react-native-paper';

import { Formik, FieldArray, ErrorMessage } from 'formik';
import { THEME } from '../../theme';
import * as Yup from 'yup';


const singleValidation = Yup.object().shape({
    question: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    answers: Yup.array().of(Yup.string().required('Поле обязательно')),
});



const AddSingleQuestion = ({ route, navigation }) => {
    const initialForm = route?.params?.question || {
        question: "",
        answers: ["Потому что ёж", "Капибара"],
        rightAnswer: ""
    }
    const submitHandler = (values) => {
        route.params.onReturn({ ...values, type: "order", rightAnswer: values.answers });
        navigation.goBack()
    }
  
    return (
        <>

            <Formik
                initialValues={initialForm}
                validationSchema={singleValidation}
                onSubmit={values => submitHandler(values)}
            >

                {({ handleChange, handleBlur, handleSubmit, values, setFieldValue, errors, touched }) => (
                    <ScrollView >
                        <View style={styles.mainForm}>
                            <Text style={styles.title} variant="titleMedium">Создание тестового этапа</Text>
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
                                                <View key={index}>
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
                                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                                        <ErrorMessage name={`answers.[${index}]`} />
                                                    </Text>
                                                </View>

                                            ))}
                                            <Button style={styles.button}
                                                onPress={() => arrayHelpers.push("")}
                                                icon="plus" mode="contained" title="Submit"
                                            >
                                                Вариант
                                            </Button>
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
        marginTop: 6,
        marginBottom: 6
    },
    input: {
        marginTop: 8,
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
    rowItem: {
        margin: 4,
        padding: 10,
        borderRadius: 4,
    },
    rowText: {
        fontSize: 24
    }
})

export default AddSingleQuestion;
