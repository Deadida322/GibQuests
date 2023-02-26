import React, { useState, useRef } from 'react';
import { Button, Text, TextInput, Card } from "react-native-paper"
import ImageLoader from '../components/UI/ImageLoader';
import {
    View,
    StyleSheet,
    ScrollView,
    TouchableWithoutFeedback,
    Keyboard
} from 'react-native';
import { THEME } from '../theme'
import { Formik, ErrorMessage } from 'formik';
import * as Yup from 'yup';
const personalValidation = Yup.object().shape({
    name: Yup.string()
        .min(2, 'Слишком короткое имя')
        .required('Поле обязательно'),
    surname: Yup.string()
        .required('Поле обязательно'),
});
const loginValidation = Yup.object().shape({
    login: Yup.string()
        .required('Поле обязательно'),
    password: Yup.string()
        .required('Поле обязательно'),
    confirmPassword: Yup.string()
        .oneOf([Yup.ref('password'), null], 'Пароль должен совпадать'),
});

const CreateScreen = ({ navigation }) => {
    const submitPersonal = (values) => {
        console.log(values)
    }
    const submitLogin = (values) => {
        console.log(values)
    }

    const initialPersonal = {

    }

    const initialLogin = {

    }

    return (
        <ScrollView>
            <TouchableWithoutFeedback onPress={() => {
                if (!Keyboard) return
                Keyboard.dismiss()
            }}>
                <View style={styles.wrapper}>
                    <Card style={styles.card}>
                        <Formik
                            initialValues={initialPersonal}
                            validationSchema={personalValidation}
                            onSubmit={values => submitPersonal(values)}
                        >
                            {({ handleChange, handleBlur, handleSubmit, values, touched, errors }) => (
                                <View>
                                    <Card.Title titleVariant="titleMedium" titleStyle={styles.title} title="Личная информация" />
                                    <Card.Content>
                                        <ImageLoader></ImageLoader>
                                        <TextInput
                                            error={touched.name && errors.name ? errors.name : ""}
                                            onChangeText={handleChange('name')}
                                            onBlur={handleBlur('name')}
                                            value={values.name}
                                            mode="outlined"
                                            label="Имя"
                                            placeholder={'Введите текст'}
                                            style={styles.input}
                                        />
                                        <TextInput
                                            mode="outlined"
                                            label="Фамилия"
                                            placeholder={'Введите текст'}
                                            error={touched.surname && errors.surname ? errors.surname : ""}
                                            onChangeText={handleChange('surname')}
                                            onBlur={handleBlur('surname')}
                                            value={values.surname}
                                            style={styles.input}
                                        />
                                        <Button onPress={handleSubmit} mode='contained-tonal' style={styles.btn}>Сохранить</Button>
                                    </Card.Content>
                                </View>)}

                        </Formik>
                    </Card>
                    <Card style={styles.card}>
                    <Card.Title titleVariant="titleMedium" titleStyle={styles.title} title="Данные для входа" />
                        <Card.Content>
                            <Formik
                                initialValues={initialLogin}
                                validationSchema={loginValidation}
                                onSubmit={values => submitLogin(values)}
                            >
                                {({ handleChange, handleBlur, handleSubmit, values, touched, errors }) => (
                                    <View>
                                        <TextInput
                                            mode="outlined"
                                            label="Логин"
                                            placeholder={'Введите текст'}
                                            style={styles.input}
                                            error={touched.login && errors.login ? errors.login : ""}
                                            onChangeText={handleChange('login')}
                                            onBlur={handleBlur('login')}
                                            value={values.login}
                                        />
                                        <TextInput
                                            mode="outlined"
                                            label="Пароль"
                                            placeholder={'Введите текст'}
                                            style={styles.input}
                                            onChangeText={handleChange('password')}
                                            error={touched.password && errors.password ? errors.password : ""}
                                            onBlur={handleBlur('password')}
                                            value={values.password}
                                        />
                                        <TextInput
                                            type="password"
                                            mode="outlined"
                                            label="Подтвердите пароль"
                                            placeholder={'Введите текст'}
                                            style={styles.input}
                                            error={touched.confirmPassword && errors.confirmPassword ? errors.confirmPassword : ""}
                                            onChangeText={handleChange('confirmPassword')}
                                            onBlur={handleBlur('confirmPassword')}
                                            value={values.confirmPassword}
                                        />
                                        <Button onPress={handleSubmit} mode='contained-tonal' style={styles.btn}>Сохранить</Button>
                                    </View>)}
                            </Formik>
                        </Card.Content>
                    </Card>
                </View>
            </TouchableWithoutFeedback>

        </ScrollView>
    );
}

const styles = StyleSheet.create({
    wrapper: {
        padding: 10
    },
    input: {
        marginVertical: 4
    },
    title: {
        color: THEME.colors.primary
    },
    card: {
        borderRadius: 4,
        marginVertical: 4
    },
    btn: {
        marginVertical: 6,
        alignSelf: "flex-end"
    }

})

export default CreateScreen;
