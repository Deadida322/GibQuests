import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';
import { ActivityIndicator, Text, Banner } from 'react-native-paper';
import ProcessError from './ProcessError';
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
        case "qrCode": {
            return <GoQR index={index} stage={stage} onNextStage={onNextStage} availableStep={availableStep}></GoQR>
        }
        case "qrСode": {
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
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const [availableStep, setAvailableStep] = useState(0)
    const [socket, setSocket] = useState("")
    const baseUrl = "ws://192.168.43.173:9007/room"
    useEffect(() => {
        request({
            url: `/ProcessQuest/ConnectToQuest`,
            method: "POST",
            headers: { 'Authorization': `Bearer ${auth.accessToken}` },
            data: {
                id: quest.id
            }
        }).then((res) => {
            const url = `${baseUrl}/${res.room}`
            const ws = new WebSocket(url)
            setSocket(ws)
            ws.onerror = (err) => console.log(err, 'err')
            ws.onopen = (e) => console.log(e, "open")
            ws.onmessage = e => {
                const res = JSON.parse(e.data)
                if(res.success){
                    setIndex(res.stage)
                    setAvailableStep(res.stage)
                    
                } else{
                    setError(res.error)
                }
                setLoading(false)
            }
        })
    }, [])
    const onNextStage = (idx, stage) => {
        setLoading(true)
        socket.send(JSON.stringify(stage))
    }
    const onGoBack = ()=>{
        setError("")
    }
    return (
        <View style={{ flex: 1 }}>
            <Tabs
                onChangeIndex={e => ()=>{setIndex(e); setError(" ")}}
                showTextLabel={true}
                style={{ backgroundColor: "transparent", height: 1200 }}
                mode="scrollable"
                defaultIndex={0}
                showLeadingSpace={true}
                disableSwipe={true}
            >
                {quest.stages.reverse().map((item, key) => (
                    <TabScreen key={key} label={item.type} disabled={key > availableStep} icon={getIcon(item.type)} false>
                        <View style={styles.goContainer}>
                            <ScrollView>
                                {loading ? 
                                    <View style={styles.loadingContainer}>
                                        <ActivityIndicator size={'large'} animating={true} />
                                        
                                    </View>
                                    : getGoComponent(item, key, onNextStage, availableStep)
                                }
                                {
                                    error ?
                                    <ProcessError idx={key} error={error} onGoBack={()=>onGoBack()}/> :''
                                }
                            </ScrollView>
                        </View>
                    </TabScreen>)
                )}

            </Tabs>
        </View>
    )
}

const styles = StyleSheet.create({
    loadingContainer:{
        marginTop: 100
    }
})
