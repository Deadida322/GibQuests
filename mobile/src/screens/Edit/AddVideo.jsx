import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Text, Button, TextInput } from 'react-native-paper';
import VideoPreview from '../../components/VideoPreview';
import { Formik, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { THEME } from '../../theme';

const validation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    url: Yup.string()
        .matches(
            /https:\/\/youtu.be\//,
            "Некорректный url"
        )
        .required('Поле обязательно')
});

const AddText = ({ route, navigation }) => {
    const initialForm = route?.params?.stage  || {
        title: "",
        url: "",
        type: "video"
    }
    
    const submitHandler = (values)=>{
        route.params.onReturn(
            {
                ...values,
            }
        );
        navigation.goBack()
    }
    return (
        <>
            <Formik
                initialValues={initialForm}
                validationSchema={validation}
                onSubmit={values => submitHandler(values)}
            >
                {({ handleChange, handleBlur, handleSubmit, values, errors, touched }) => (
                    <View style={styles.mainForm}>
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
                            label="Ссылка на youtube"
                            mode="outlined"
                            style={styles.input}
                            onChangeText={handleChange('url')}
                            error={touched.url && errors.url ? errors.url : ""}
                            onBlur={handleBlur('url')}
                            value={values.url}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="url" />
                        </Text>
                        <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Добавить</Button>
                        <Text style={styles.title} variant="titleMedium">Предпросмотр:</Text>
                        <VideoPreview stage={values}></VideoPreview>
                    </View>
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
