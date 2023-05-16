import React, {useState, useEffect} from 'react';
import { View, StyleSheet, ImageBackground } from 'react-native';
import {Button} from "react-native-paper"
import * as ImagePicker from 'expo-image-picker';
const srcImg = "https://img.freepik.com/free-vector/happy-people-air-balloon-flat-vector-illustration-man-woman-employees-looking-through-binoculars-searching-creative-vacancies-finding-work-job-hunting-research-concept_74855-24259.jpg?w=1380&t=st=1676117095~exp=1676117695~hmac=3d9bfb8f0bd99c96c9e641cf0cb62edf82a19953e2fc0e0712c692600be734a2"

export default ImageLoader = ({img, onImageLoad}) => {
    const initialImg = img || srcImg
    const [image, setImage] = useState(initialImg)
    useEffect(()=>{
        console.log(img)
        if(!img){
            setImage(srcImg)
        } else{
            setImage(img)
        }
    }, [img])

    const makePhoto = async () => {
        const result = await ImagePicker.launchCameraAsync({
            quality: .2,
            allowsEditing: true,
            base64: true
        })

        if (!result.canceled) {
            const data = result.assets[0].uri
            setImage(data)
            onImageLoad(data)
        }
    }
    const pickImage = async () => {
        let result = await ImagePicker.launchImageLibraryAsync({
            mediaTypes: ImagePicker.MediaTypeOptions.All,
            allowsEditing: true,
            base64: true,
            quality: .2,
        });

        if (!result.canceled) {
            const data = "data:image/jpeg;base64," + result.assets[0].base64
            setImage(data)
            onImageLoad(data)
        }
    };

    return (
        <ImageBackground source={{ uri: image }} style={styles.imageBackground}>
            <View style={styles.buttonsContainer}>
                <Button style={styles.imageBtn} icon="camera-burst" mode="elevated" onPress={() => pickImage()}>Выбрать из галереи</Button>
                <Button style={styles.imageBtn} icon="camera-outline" mode="contained-tonal" onPress={() => makePhoto()}>Сделать фото</Button>
            </View>
        </ImageBackground>
    );
}

const styles = StyleSheet.create({
    imageBackground: {
        flex: 1,
        height: 200,
        borderRadius: 6,
        overflow: "hidden",
        marginTop: 6,
        marginBottom: 6,
        borderWidth: 1,
        borderColor: "rgba(102, 77, 98, .95)",

    },
    buttonsContainer: {
        flex: 1,
        opacity: .9,
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: "rgba(73, 44, 68, 0.2)",
    },
    imageBtn: {
        width: "60%",
        margin: 8,
        opacity: .9
    }
})

