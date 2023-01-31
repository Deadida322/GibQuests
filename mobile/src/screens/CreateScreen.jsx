import React, {useState, useRef} from 'react';
import { useDispatch } from 'react-redux';
import { createPost } from '../store/actions/post';
import PhotoPicker from '../components/PhotoPicker';
import {
    View, 
    Text, 
    StyleSheet, 
    TextInput, 
    Image, 
    Button, 
    ScrollView, 
    TouchableWithoutFeedback,
    Keyboard
} from 'react-native';
import {THEME} from '../theme'

const CreateScreen = ({navigation}) => {
    const imgRef = useRef()
    const dispatch = useDispatch()
    const [text, setText] = useState('')
    const pressHandler = ()=>{
        const post = {
            date: new Date().toJSON(),
            text,
            img: imgRef.current, 
            booked: false
        }
        dispatch(createPost(post))
        navigation.navigate('Main')
    }
    const photoPickHandler = (uri)=>{
        console.log(uri)
        imgRef.current = uri
    }
    return (
        <ScrollView>
            <TouchableWithoutFeedback onPress={()=> {
                    if (!Keyboard) return
                    Keyboard.dismiss()
                }
            }>
                <View style={styles.wrapper}>
                <Text style={styles.title}>Создание нового поста</Text>
                    <TextInput 
                        multiline
                        value={text} 
                        placeholder={'Введите текст'} 
                        onChangeText={e=>setText(e)}
                        style={styles.textarea}
                    />
                    <PhotoPicker onPick={photoPickHandler}></PhotoPicker>
                    <Button disabled={!imgRef.current || !text} title="Создать" onPress={pressHandler} color={THEME.PRIMARY_COLOR}></Button>
                </View>
            </TouchableWithoutFeedback>

        </ScrollView>
    );
}

const styles = StyleSheet.create({
    wrapper:{
        padding: 10
    },
    title: {
        fontSize: 20,
        textAlign: "center",
        fontFamily: 'regular',
        marginVertical: 10
    },
    textarea:{
        padding: 10,
        marginBottom: 10,
        
    }
})

export default CreateScreen;
