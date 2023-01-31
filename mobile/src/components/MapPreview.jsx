import React, { useEffect, useState, useRef } from 'react';
import { StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import MapView, { Marker } from 'react-native-maps';


const MapPreview = ({ stage, currentPosition, setMapPosition, mapPosition }) => {
    let mapRef = useRef(null)

    useEffect(() => {
        mapRef?.current?.getCamera().then((cam) => {
            cam.center = currentPosition
            mapRef?.current?.animateCamera(cam);
        });

    }, [])
    useEffect(() => {
        mapRef?.current?.getCamera().then((cam) => {
            cam.center = mapPosition
            mapRef?.current?.animateCamera(cam);
        });

    }, [mapPosition])

    return (
        <>
            <Card style={styles.card}>
                <Card.Title title={stage?.title || "Текстовый этап"}></Card.Title>
                <Card.Content>
                    <MapView
                        onPress={(e) => {
                            setMapPosition(e.nativeEvent.coordinate)
                        }}
                        initialRegion={{
                            ...currentPosition,
                            latitudeDelta: 0.0922,
                            longitudeDelta: 0.0421,
                        }}
                        showsUserLocation={true}
                        userLocationPriority={'high'}
                        minZoomLevel={8}
                        ref={mapRef}
                        style={styles.map}
                    >
                        <Marker
                            title="Текущая точка"
                            coordinate={{ ...currentPosition }}
                        />
                        {mapPosition ? <Marker
                            title="Точка с карты"
                            coordinate={{ ...mapPosition }}
                        /> : ""}
                    </MapView>
                </Card.Content>
            </Card>
        </>
    );
}

const styles = StyleSheet.create({
    card: {
        margin: 4,
        borderRadius: 2,
        marginTop: 8,
        marginBottom: 36,
    },
    map: {
        width: '100%',
        height: 360,
    },
})

export default MapPreview;
