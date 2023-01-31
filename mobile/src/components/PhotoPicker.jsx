import React, {useState} from 'react';
import {View, StyleSheet, Image, Button, Alert, Text} from 'react-native';
import * as ImagePicker from 'expo-image-picker'
import * as Permissions from 'expo-permissions'
async function askForPermissions(){
    console.log(Permissions)
    const {status} = await Permissions.askAsync(
        Permissions.CAMERA,
        Permissions.CAMERA_ROLL
    )
    if(status!='granted'){
        Alert("Ошибка прав", "Вы запрещаете мне добавлять фото")
        return false
    }
    return true
}
const PhotoPicker = ({onPick}) => {
    const [image, setImage] = useState(null)
    const addPhoto = async ()=>{
        const hasPermissions = await askForPermissions()
        if(!hasPermissions) {
            return false
        }
        const img = await ImagePicker.launchCameraAsync({
            quality: .7,
            allowsEditing: true,
            aspect: [16, 9]
        })
        setImage(img.uri)
        onPick(img.uri)
    }
    return (
        <View style={styles.wrapper}>
            <Button onPress={addPhoto} title="Добавить фото"></Button>
            {image && <Image style={styles.image} source={{uri:image}}></Image>}
        </View>
    );
}

const styles = StyleSheet.create({
    wrapper: {
        marginBottom: 10
    },
    image:{
        marginTop: 10,
        width:"100%",
        height: 200
    }
})

export default PhotoPicker;
