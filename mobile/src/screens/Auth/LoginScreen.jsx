import React, { useState, useEffect } from 'react';
import { View, StyleSheet, Image, ScrollView } from 'react-native';
import { Card, Button, Text, TextInput } from "react-native-paper"
import { Formik, ErrorMessage } from 'formik';
import ShowError from '../../components/UI/ShowError';
import * as Yup from 'yup';
import { ToastAndroid } from "react-native"
import { THEME } from '../../theme';
import Auth from "../../store/auth"
import AsyncStorage from '@react-native-async-storage/async-storage';

import { Observer, observer } from "mobx-react-lite"


const loginValidation = Yup.object().shape({
    login: Yup.string()
        .required('Поле обязательно'),
    password: Yup.string()
        .required('Поле обязательно'),
});

const LoginScreen = ({ navigation, route }) => {
    const [show, setShow] = useState(false)
    useEffect(() => {
        const getItem = async () => {

            const user = await AsyncStorage.getItem("user")
            if (user) {
                Auth.tryToPair()
                    .then(res => {
                        console.log("user")
                        navigation.navigate("DrawerNavigation")
                    })
                    .finally(err => {
                        setShow(true)
                    })
            } else {
                setShow(true)
            }
            
        }
        getItem()

    }, [])
    const [showError, setShowError] = useState(false)

    const submitLogin = (data) => {
        Auth.login(data)
            .then(res => {
                console.log(res)
                navigation.navigate("DrawerNavigation")
            })
            .catch(() => {
                setShowError(true)
            })
    }
    if (!show) {
        return
    }

    return (
        <ScrollView >
            <View style={styles.wrapper}>
                <ShowError visible={showError} error={""} setVisible={setShowError}></ShowError>
                <Image
                    style={{ width: 180, resizeMode: 'contain', height: 250 }} source={require("../../../assets/logo.png")} />
                <Card style={styles.card}>
                    <Card.Title title="Войдите в аккаунт" />
                    <Card.Content>
                        <Formik
                            initialValues={{}}
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
                                    <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                                        <ErrorMessage name="login" />
                                        {store.loading}
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
                                    <Observer>
                                        {() => (
                                            <Card.Actions>
                                                <Text>{Auth.loading}</Text>
                                                <Button onPress={() => navigation.navigate("RegisterScreen")} mode='contained-tonal' style={styles.btn}>Регистрация</Button>
                                                <Button disabled={Auth.loading} onPress={handleSubmit} mode='contained' style={styles.btn}>Вход</Button>
                                            </Card.Actions>)}
                                    </Observer>

                                </View>)}
                        </Formik>
                    </Card.Content>
                </Card>
            </View>

        </ScrollView>
    );
}

const styles = StyleSheet.create({
    wrapper: {
        display: "flex",
        justifyContent: "flex-start",
        alignItems: "center",
        flex: 1,
        padding: 10
    },
    input: {
        marginVertical: 4
    },
    title: {
        textAlign: "center",
    },
    card: {
        borderRadius: 4,
        marginVertical: 4,
        width: "100%"
    },
    btn: {
        marginVertical: 6,
        alignSelf: "flex-end"
    }
})

export default LoginScreen;
