import React, { useState } from 'react';
import { View, StyleSheet, Text } from 'react-native';
import { Searchbar } from 'react-native-paper';
import QuestItem from '../components/QuestItem';
const quests = [
    {
        title: "Найди меня",
        description: "lorem ipsum dolor sit amet sab grou derumo matis, silico der toilo potoroto yomokato",
        img: "https://avatars.mds.yandex.net/i?id=00bbea24f4ab1b3637add8fc4e244d57cb045c21-7674757-images-thumbs&n=13",

        stages: [
            {
                title: "test",
                type: "test",
                questions: [
                    {
                        title: "Сколько хош",
                        type: "insert",
                        answers: [
                            "адин",
                        ],
                        rightAnswer: [
                            "адин",
                            
                        ]
                    },
                    {
                        title: "Сколько хош",
                        type: "single",
                        answers: [
                            "адин",
                            "два",
                            "три"
                        ],
                        rightAnswer: [
                            "адин"
                        ]
                    },
                    {
                        title: "Сколько хош",
                        type: "multiple",
                        answers: [
                            "адын",
                            "Два",
                            "Три"
                        ],
                        rightAnswer: [
                            "адин",
                            "Два"
                        ]
                    },
                    {
                        title: "Сколько хош",
                        type: "order",
                        answers: [
                            "Четыре",
                            "Два",
                            "Три"
                        ],
                        rightAnswer: [
                            "Четыре",
                            "Два",
                            "Три"
                        ]
                    },
                ]
            },
            {
                title: "qr",
                type: "qrcode",
                code: "asdasdsda"
            },
            {
                title: "Youtube",
                type: "video",
                url: "asdasdsda"
            },
            {
                title: "Первый этап",
                type: "text",
                description: "asdasdsda"
            },
            {
                title: "Найди дорогу домой",
                type: "map",
                description: "asdasdsda",
                coords:{
                    longitude: 48.3971502,
                    latitude: 54.3438362
                }
            },
            {
                title: "СНова текст",
                type: "text",
                description: "LoremDolar sit amet"
            },
            {
                title: "Просмотри",
                type: "video",
                url: "asdasdsda"
            },
        ]
    }
]

const AboutScreen = ({navigation, route}) => {
    const [searchQuery, setSearchQuery] = React.useState('');
    const onChangeSearch = query => setSearchQuery(query);
    const onQuestPreview = (quest) => {
        navigation.navigate("QuestPreview", {quest: quest})
    }
    return (
        <View style={styles.main}>
            <Searchbar
                placeholder="Введите код"
                onChangeText={onChangeSearch}
                value={searchQuery}
            />
            <Text style={styles.title} variant="bodyMedium">Популярные</Text>
            {quests.map(item => (
                <QuestItem onQuestPreview={()=>onQuestPreview(item)} quest={item} />)
            )}
        </View>
    );
}

const styles = StyleSheet.create({
    main: {
        padding: 10
    },
    title: {
        marginTop: 6,
        marginBottom: 6
    }
})

export default AboutScreen;
