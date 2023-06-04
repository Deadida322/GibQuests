import React, { useEffect } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import { Button, Text, Card } from "react-native-paper"
import QuestItem from '../../components/QuestItem';
import WatchProgress from '../../components/Watch/WatchProgress';
import request from '../../request';
import { THEME } from '../../theme';

const quest = {
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
            coords: {
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

const progress = [{
    user: {
        login: "deadida32",
        name: "Давид",
        surname: "Алиев"
    },
    progress: {
        stage: 6,
        stageName: "Полюбить всех пчёл",
        currentTime: "00:00"
    }
}]

const WatchScreen = ({ navigation, route }) => {
   
    return (
        <View style={styles.wrapper}>
            <QuestItem quest={quest} shortInfo={true}></QuestItem>
            <Text variant='titleMedium' style={styles.title}>На данный момент проходят</Text>
            <ScrollView>
                <WatchProgress progress={progress[0]} count={quest.stages.length}></WatchProgress>
            </ScrollView>
        </View>
    );
}

const styles = StyleSheet.create({
    wrapper: {
        padding: 8
    },
    title: {
        marginVertical: 6,
        color: THEME.colors.primary
    }
})

export default WatchScreen;
