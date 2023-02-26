import React from 'react';
import { View, StyleSheet, Image, ScrollView } from 'react-native';
import { Card, Button, Text, TextInput } from "react-native-paper"
import { Formik, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { THEME } from '../../theme';


const loginValidation = Yup.object().shape({
    login: Yup.string()
        .required('Поле обязательно'),
    password: Yup.string()
        .required('Поле обязательно'),
});

const LoginScreen = ({ navigation, route }) => {
    const submitLogin = (data) => {
        navigation.navigate("DrawerNavigation")
    }
    return (
        <ScrollView >
            <View style={styles.wrapper}>
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
                                    <Card.Actions>
                                        <Button onPress={() => navigation.navigate("RegisterScreen")} mode='contained-tonal' style={styles.btn}>Регистрация</Button>
                                        <Button onPress={handleSubmit} mode='contained' style={styles.btn}>Вход</Button>
                                    </Card.Actions>

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
