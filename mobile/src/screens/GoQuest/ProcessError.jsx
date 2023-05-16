import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Text, Banner, Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';

const ProcessError = ({error, onGoBack}) => {
    const goTo = useTabNavigation();
    const idx = useTabIndex()
    const transition = () => {
        onGoBack()
        goTo(idx - 1)
    }
    return (
        <Banner
            visible={!!error}
            style={
                {
                    margin: 10
                }
            }
            icon={() => (
                <Ionicons name="alert-circle-outline" size={32} color="blue" />
            )}
        >
                <View style={{width: '100%', justifyContent: 'center'}}>
                    <Text style={{width: '100%'}}>
                        {error}
                    </Text>
                    <View style={{ marginTop: 8, width: '100%', justifyContent: 'flex-end'}}>
                        <Button onPress={()=>transition()} mode='contained-tonal'>Назад</Button>
                    </View>
                </View>
           
           
        </Banner>
    );
}

const styles = StyleSheet.create({})

export default ProcessError;
