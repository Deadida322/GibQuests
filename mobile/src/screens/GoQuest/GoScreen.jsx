import { Button, Title, Paragraph, Text } from 'react-native-paper';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';
import auth from "../../store/auth"
import { View, StyleSheet, ScrollView, } from 'react-native';
import GoMap from '../../components/GoQuest/GoMap';
import { useEffect, useState } from "react"
import GoText from "../../components/GoQuest/GoText"
import GoVideo from '../../components/GoQuest/GoVideo';
import GoQR from '../../components/GoQuest/GoQR';
import GoTest from '../../components/GoQuest/GoTest/index';
import request from "../../request"
const getIcon = (type) => {
    switch (type) {
        case "text": {
            return 'card-text-outline'
        }
        case "video": {
            return 'youtube'
        }
        case "qrcode": {
            return 'qrcode'
        }
        case "map": {
            return 'map-outline'
        }
        case "test": {
            return 'school-outline'
        }
    }
}

const getGoComponent = (stage, index, onNextStage, availableStep) => {
    switch (stage.type) {
        case "text": {
            return <GoText index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoText>
        }
        case "video": {
            return <GoVideo index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoVideo>
        }
        case "map": {
            return <GoMap index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoMap>
        }
        case "qrcode": {
            return <GoQR index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoQR>
        }
        case "test": {
            return <GoTest index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoTest>
        }
        default: {
            return <View style={{ flex: 1 }}></View>
        }
    }
}

export default function GoScreen({ navigation, route }) {
    const quest = route.params.quest
    const [index, setIndex] = useState(useTabIndex())
    const [availableStep, setAvailableStep] = useState(0)
    const [socket, setSocket] = useState("")
    const baseUrl = "wss://192.168.43.173:9007/api/room"
    useEffect(()=>{
        request({
            url: `/ProcessQuest/ConnectToQuest`,
            method: "POST",
            headers: { 'Authorization': `Bearer ${auth.accessToken}` },
            data: {
                id: quest.id
            }
        }).then((res)=>{
            console.log(res)
        })
        // const ws = new WebSocket('ws://host.com/path');
        // setSocket(ws)
    }, [])
    const onNextStage = (idx) => {
        setIndex(idx)
        setAvailableStep(idx)
    }
    return (
        <View style={{ flex: 1 }}>
            <Tabs
                onChangeIndex={e => setIndex(e)}
                showTextLabel={true}
                style={{ backgroundColor: "transparent", height: 1200 }}
                mode="scrollable"
                defaultIndex={0}
                showLeadingSpace={true}
                disableSwipe={true}
            >
                {quest.stages.map((item, key) => (
                    <TabScreen key={key} label={item.type} disabled={key > availableStep} icon={getIcon(item.type)} false>
                        <View style={styles.goContainer}>
                            <ScrollView>
                                {getGoComponent(item, key, onNextStage, availableStep)}
                            </ScrollView>
                        </View>
                    </TabScreen>)
                )}

            </Tabs>
        </View>
    )
}

const styles = StyleSheet.create({
    
})
