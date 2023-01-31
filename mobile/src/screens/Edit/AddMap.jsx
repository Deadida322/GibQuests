import React, { useEffect, useState } from 'react';
import { StyleSheet, ScrollView } from 'react-native';
import { Text, Button, TextInput } from 'react-native-paper';
import MapPreview from '../../components/MapPreview';
import { Formik, ErrorMessage } from 'formik';
import * as Location from 'expo-location';
import * as Yup from "yup"
import { THEME } from '../../theme';

const validation = Yup.object().shape({
    title: Yup.string()
        .min(2, 'Слишком короткое название')
        .max(60, 'Слишком длинное название')
        .required('Поле обязательно'),
});


const AddMap = ({ route, navigation }) => {
    const initialForm = route?.params?.stage || {
        title: "",
        description: ""
    }

    let coords = route?.params?.stage?.coords || {
        latitude: 0,
        longitude: 0
    };

    const submitHandler = (values) => {
        route?.params?.onReturn({
            ...values,
            type: "map",
            coords: currentPosition
        })
        navigation.goBack()
    }

    const [myPosition, setMyPosition] = useState(coords)
    const [currentPosition, setCurrentPosition] = useState(coords)
    const [mapPosition, setMapPosition] = useState(null)

    const askPos = async () => {
        let loc = await Location.getCurrentPositionAsync(
            {
                accuracy: Location.Accuracy.High,
                distanceInterval: 10,
            }
        )
        setMyPosition(loc.coords)
    }

    useEffect(() => {
        (async () => {
            const foregroundPermission = await Location.requestForegroundPermissionsAsync()
            if (foregroundPermission.granted) {
                askPos()
            }
        })();

    }, []);

    const setPositionToMap = (e) => {
        setCurrentPosition(e)
    }


    return (
        <>
            <Formik
                validationSchema={validation}
                initialValues={initialForm}
                onSubmit={values => submitHandler(values)}
            >
                {({ handleChange, handleBlur, handleSubmit, values, touched, errors }) => (
                    <ScrollView style={styles.mainForm}>
                        <Text style={styles.title} variant="titleMedium">Создание этапа геолокации</Text>
                        <TextInput
                            label="Название"
                            mode="outlined"
                            onChangeText={handleChange('title')}
                            onBlur={handleBlur('title')}
                            error={touched.title && errors.title ? errors.title : ""}
                            value={values.title}
                            style={styles.button}
                        />
                        <Text style={{ color: THEME.colors.error }} variant="labelSmall">
                            <ErrorMessage name="title" />
                        </Text>
                        <TextInput
                            label="Текущая точка"
                            mode="outlined"
                            disabled={true}
                            onChangeText={handleChange('title')}
                            style={styles.button}
                            onBlur={handleBlur('title')}
                            value={`${myPosition.latitude}; ${myPosition.longitude}`}
                        />
                        <Button style={styles.button} onPress={() => setCurrentPosition(myPosition)} mode="contained">Отметить мою</Button>
                        <Button style={styles.button} onPress={() => setPositionToMap(mapPosition)} mode="outlined">Отметить с карты</Button>
                        <Button style={styles.button} onPress={handleSubmit} mode="contained-tonal" title="Submit" >Сохранить</Button>
                        <Text style={styles.title} variant="titleMedium">Предпросмотр: </Text>
                        <MapPreview currentPosition={currentPosition} setMapPosition={setMapPosition} mapPosition={mapPosition}></MapPreview>
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
        marginBottom: 8
    }
})

export default AddMap;
