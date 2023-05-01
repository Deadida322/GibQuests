import React, { useState, useEffect } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Card, Button, Text, TextInput } from "react-native-paper"
import { Formik, ErrorMessage } from 'formik';
import ImageLoader from '../../components/UI/ImageLoader';
import ShowError from '../../components/UI/ShowError';
import ShowSuccess from '../../components/UI/ShowSuccess'
import * as Yup from 'yup';
import { THEME } from '../../theme';
import request from '../../request';

const loginValidation = Yup.object().shape({
    login: Yup.string()
        .required('Поле обязательно'),
    password: Yup.string()
        .required('Поле обязательно'),
    confirmPassword: Yup.string().oneOf([Yup.ref('password'), null], 'Пароли должны совпадать')
});

const RegisterScreen = ({ navigation, route }) => {

    const [image, setImage] = useState("")
    const [showError, setShowError] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState("")
    const [success, setSuccess] = useState(false)

    const initialValues = {
        name: "",
        surname: "",
        login: "",
        password: "",
        confirmPassword: ""
    }

    const submitLogin = (data) => {
        const info = {
            ...data,
            userName: data.login,
            img: image
        }
        setIsLoading(true)
        request({
            url: '/Auth/Register',
            method: 'POST',
            data: info
        }).then((res) => {
            setIsLoading(false)
            setSuccess(true)
            // navigation.navigate("LoginScreen")
        }).catch((err) => {
            console.log(err)
            setError(err.join(" "))
            setShowError(true)
            setIsLoading(false)
        });
    }

    return (
        <ScrollView>
            <View style={styles.wrapper}>
            <ShowError visible={showError} error={error} setVisible={setShowError}></ShowError>
            <ShowSuccess visible={success} message={"Теперь вы можете выполнить вход в приложение"} setVisible={setSuccess}></ShowSuccess>
                <Formik
                    initialValues={initialValues}
                    validationSchema={loginValidation}
                    onSubmit={values => submitLogin(values)}
                >
                    {({ handleChange, handleBlur, handleSubmit, validateForm, values, touched, errors }) => (
                        <View style={{ width: "100%" }}>
                            <Card style={styles.card}>
                                <Card.Title title="Новый аккаунт" />
                                <Card.Content>
                                    <ImageLoader img={image} onImageLoad={(e) => setImage(e)}></ImageLoader>
                                    <TextInput
                                        mode="outlined"
                                        label="Имя"
                                        placeholder={'Введите текст'}
                                        style={styles.input}
                                        error={touched.name && errors.name ? errors.name : ""}
                                        onChangeText={handleChange('name')}
                                        onBlur={handleBlur('name')}
                                        value={values.name}
                                    />
                                    <TextInput
                                        mode="outlined"
                                        label="Фамилия"
                                        placeholder={'Введите текст'}
                                        style={styles.input}
                                        error={touched.surname && errors.surname ? errors.surname : ""}
                                        onChangeText={handleChange('surname')}
                                        onBlur={handleBlur('surname')}
                                        value={values.surname}
                                    />
                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                        <ErrorMessage name="surname" />
                                    </Text>
                                </Card.Content>
                            </Card>
                            <Card style={{ ...styles.card, marginTop: 10 }}>
                                <Card.Title title="Информация для входа"></Card.Title>
                                <Card.Content>
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
                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                        <ErrorMessage error={error} name="login" />
                                    </Text>
                                    <TextInput
                                        mode="outlined"
                                        label="Пароль"
                                        placeholder={'Введите текст'}
                                        style={styles.input}
                                        onChangeText={handleChange('password')}
                                        error={touched.password && errors.password ? errors.password : ""}
                                        onBlur={handleBlur('password')}
                                        value={values.password}
                                        secureTextEntry={true}
                                    />
                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                        <ErrorMessage name="password" />
                                    </Text>
                                    <TextInput
                                        mode="outlined"
                                        label="Повторите пароль"
                                        placeholder={'Введите текст'}
                                        style={styles.input}
                                        onChangeText={handleChange('confirmPassword')}
                                        error={touched.confirmPassword && errors.confirmPassword ? errors.confirmPassword : ""}
                                        onBlur={handleBlur('confirmPassword')}
                                        value={values.confirmPassword}
                                        secureTextEntry={true}
                                    />
                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                        <ErrorMessage name="confirmPassword" />
                                    </Text>
                                    <Card.Actions>
                                        <Button onPress={() => navigation.navigate("LoginScreen")} mode='contained-tonal' style={styles.btn}>Войти</Button>
                                        <Button disabled={isLoading} onPress={() => { validateForm(); handleSubmit() }} mode='contained' style={styles.btn}>Регистрация</Button>
                                    </Card.Actions>
                                </Card.Content>
                            </Card>
                        </View>
                    )}
                </Formik>
            </View>

        </ScrollView >
    );
}

const styles = StyleSheet.create({
    wrapper: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        flex: 1,
        padding: 10,
        marginVertical: 10
    },
    input: {
        marginVertical: 4
    },
    title: {
        textAlign: "center",
    },
    card: {
        width: "100%",
        borderRadius: 4,
        marginVertical: 4
    },
    btn: {
        marginVertical: 6,
        alignSelf: "flex-end"
    }
})

export default RegisterScreen;
