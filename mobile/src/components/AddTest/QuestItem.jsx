import React from 'react';
import {View, StyleSheet} from 'react-native';
import {Card, Button, Avatar} from "react-native-paper"
import { MaterialCommunityIcons } from '@expo/vector-icons';
import {THEME} from "../../theme"

const getIcon = (type)=>{
    switch(type){
        case "single":{
            return 'help'
        }
        case "multiple":{
            return 'beaker-question-outline'
        }
        case "insert":{
            return 'content-paste'
        }
        case "order":{
            return 'reorder-horizontal'
        }
    }
}


const QuestItem = ({question, index, onQuestionEdit, onQuestionDelete}) => {

    return (
        <Card style={styles.card}>
            <Card.Title title={question.title} 
                left={() =><Avatar.Icon size={42} icon={getIcon(question.type)}></Avatar.Icon>} subtitle={index + " Вопрос"}
            />
   
            <Card.Actions>
                <Button onPress={onQuestionEdit} mode="elevated"><MaterialCommunityIcons name="pencil" size={22} color={THEME.colors.primary} /></Button>
                <Button onPress={onQuestionDelete} mode="elevated" buttonColor={THEME.colors.error}><MaterialCommunityIcons color="white"  name="delete" size={22}  /></Button>
            </Card.Actions>
        </Card>
    );
}

const styles = StyleSheet.create({
    card:{
        borderRadius: 2,
        margin: 4,
        marginBottom: 10
    }
})

export default QuestItem;
