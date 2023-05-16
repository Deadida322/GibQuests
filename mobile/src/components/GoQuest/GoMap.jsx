import { Button, Title, Paragraph, Text, Card, Chip, IconButton } from 'react-native-paper';
import { View, StyleSheet } from 'react-native';
import * as Location from 'expo-location';
import { useState, useRef, useEffect } from "react"
import { useTabIndex, useTabNavigation } from 'react-native-paper-tabs';
import MapView, { Marker } from 'react-native-maps';

export default function GoMap({ stage, index, onNextStage }) {
    const goTo = useTabNavigation();
    const idx = useTabIndex()
    const mapRef = useRef(null)
    const [isValidLocation, setIsValidLocation] = useState(false)


    const [myPosition, setMyPosition] = useState({
        longitude: 0,
        latitude: 0
    })

    const checkLocation = (coords) => {
        console.log( Math.abs(myPosition.longitude - stage.coords.longitude))
        if ((Math.abs(myPosition.latitude - stage.coords.latitude) < 0.0001500) 
            && Math.abs(myPosition.longitude - stage.coords.longitude) < 0.0001500) {
            setIsValidLocation(true)
        } else{
            setIsValidLocation(false)
        }
    }

    useEffect(() => {
        (async () => {
            const foregroundPermission = await Location.requestForegroundPermissionsAsync()
            if (foregroundPermission.granted) {
                let location = await Location.watchPositionAsync(
                    { accuracy: Location.Accuracy.High },
                    (loc) => {
                        setMyPosition(loc.coords)
                        checkLocation(myPosition)
                    }
                );
            }
        })();
    }, []);

    const transition = () => {
        onNextStage(idx + 1, stage)
        goTo(idx + 1)
    }

    const zoomToMark = () => {
        mapRef?.current?.getCamera().then((cam) => {
            cam.center = stage.coords
            mapRef?.current?.animateCamera(cam);
        });
    }

    return (
        <Card style={styles.card}>
            <Text style={styles.chip}><Chip icon="map-outline">Этап {index + 1}</Chip></Text>
            <Card.Title title={stage.title} />

            <Card.Content>
                
                <View style={{ width: "100%" }}>
                    <MapView
                        initialRegion={{
                            ...stage.coords,
                            latitudeDelta: 0.0922,
                            longitudeDelta: 0.0421,
                        }}
                        showsUserLocation={true}
                        userLocationPriority={'high'}
                        minZoomLevel={8}
                        ref={mapRef}
                        style={styles.map}
                    >
                        {stage.coords ? <Marker
                            title="Точка назначения"
                            coordinate={{ ...stage.coords }}
                        /> : ""}

                    </MapView>
                    <Button
                        mode="contained-tonal"
                        style={styles.zoomButton}
                        icon="map-marker-down"
                        onPress={() => zoomToMark()}
                    >Показать</Button>
                </View>
            </Card.Content>
            <Card.Actions>
                <Button>Назад</Button>
                <Button disabled={!isValidLocation} onPress={() => transition()}>Далее</Button>
            </Card.Actions>
        </Card>
    )
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 6,
        margin: 4,
        marginTop: 20
    },
    title: {
        marginBottom: -4
    },
    map: {
        width: '100%',
        height: 360,
    },
    zoomButton: {
        position: 'absolute',
        bottom: 14,
        opacity: .8,
        left: "30%"
    },
    chip:{
        position: "absolute",
        top: -15,
        right: 0,
    }
})
