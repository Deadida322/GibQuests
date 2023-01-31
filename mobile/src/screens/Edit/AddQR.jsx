import React from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Text, Button, TextInput } from 'react-native-paper';
import { Formik, ErrorMessage } from 'formik';
import * as Yup from "yup";
import { THEME } from '../../theme';
import QRPreview from '../../components/QRPreview';


const validation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    code: Yup.string()
        .min(2, 'Слишком короткий код')
        .required('Поле обязательно')
});

const AddText = ({ route, navigation }) => {
    const submitHandler = (values) => {
        route.params.onReturn(
            {
                ...values,
            }
        );
        navigation.goBack()
    }

    const initialForm = route?.params?.stage || {
        title: "",
        code: "",
        type: "qrcode"
    }

    return (
        <>
            <Formik
                validationSchema={validation}
                initialValues={initialForm}
                onSubmit={values => submitHandler(values)}
            >
                {({ handleChange, handleBlur, handleSubmit, values, errors, touched }) => (
                    <ScrollView style={styles.mainForm}>
                        <Text style={styles.title} variant="titleMedium">Создание видео этапа</Text>
                        <TextInput
                            label="Название этапа"
                            mode="outlined"
                            style={styles.input}
                            onChangeText={handleChange('title')}
                            error={touched.title && errors.title ? errors.title : ""}
                            onBlur={handleBlur('title')}
                            value={values.title}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="title" />
                        </Text>
                        <TextInput
                            label="Кодовое слово"
                            mode="outlined"
                            style={styles.input}
                            error={touched.code && errors.code ? errors.code : ""}
                            onChangeText={handleChange('code')}
                            onBlur={handleBlur('code')}
                            value={values.code}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="code" />
                        </Text>
                        <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Сохранить</Button>
                        <Text style={styles.title} variant="titleMedium">Предпросмотр:</Text>
                        <QRPreview stage={values}></QRPreview>
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
        marginTop: 10
    }
})

export default AddText;
