import React, { useState } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Button, Text, TextInput, Card, } from 'react-native-paper';
import { Formik, ErrorMessage } from 'formik';
import { THEME } from '../../theme';
import * as Yup from 'yup';


const singleValidation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    rightAnswer: Yup.array().of(Yup.string().required('Поле обязательно'))
        .min(1, 'Укажите верный ответ'),
});


const AddSingleQuestion = ({ route, navigation }) => {
    const initialForm = route.params?.title || {
        title: "",
        answers: [""],
        rightAnswer: [""]
    }
    const submitHandler = (values) => {
        route.params.onReturn(
            {
                ...values,
                type: "insert",
                rightAnswer: values.rightAnswer,
                answers: values.rightAnswer
            }
        );
        navigation.goBack()
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
                            <Text style={styles.title} variant="titleMedium">Вопрос с ручным вводом</Text>
                            <TextInput
                                error={touched.title && errors.title ? errors.title : ""}
                                label="Вопрос"
                                mode="outlined"
                                style={styles.input}
                                onChangeText={handleChange('title')}
                                onBlur={handleBlur('title')}
                                value={values.title}
                            />
                            <TextInput
                                error={touched.rightAnswer && errors.rightAnswer ? errors.rightAnswer : ""}
                                label="Верный ответ"
                                mode="outlined"
                                style={styles.input}
                                onChangeText={handleChange('rightAnswer.0')}
                                onBlur={handleBlur('rightAnswer.0')}
                                value={values.rightAnswer[0]}
                            />
                            <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                <ErrorMessage name="rightAnswer" />
                            </Text>

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
    }
})

export default AddSingleQuestion;
