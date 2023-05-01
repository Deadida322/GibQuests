import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Text, Button, TextInput } from 'react-native-paper';
import TextPreview from '../../components/TextPreview';
import { Formik, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { THEME } from '../../theme';


const validation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
    text: Yup.string()
        .min(10, "Описание должно быть содержательным")
        .required('Поле обязательно')
});




const AddText = ({ navigation, route }) => {
    const initialForm = route?.params?.stage || {
        title: "",
        text: "",
        type: "text"
    }
    const submitHandler = (values)=>{
        console.log(values)
        if(values){
            route.params.onReturn(
                values,
            );
        }
       
        navigation.goBack()
    }

    return (
        <>
            <Formik
                initialValues={initialForm}
                validationSchema={validation}
                onSubmit={values => submitHandler(values)}
            >
                {({ handleChange, handleBlur, handleSubmit, values, touched, errors }) => (
                    <View style={styles.mainForm}>
                        <Text style={styles.title} variant="titleMedium">Создание текстового этапа</Text>
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
                        <TextInput
                            label="Описание"
                            mode="outlined"
                            numberOfLines={4}
                            multiline={true}
                            error={touched.text && errors.text ? errors.text : ""}
                            style={styles.input}
                            onChangeText={handleChange('text')}
                            onBlur={handleBlur('text')}
                            value={values.text}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="text" />
                        </Text>
                        <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Добавить</Button>
                        <Text style={styles.title} variant="titleMedium">Предпросмотр:</Text>
                        <TextPreview stage={values}></TextPreview>
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
    }
})

export default AddText;
