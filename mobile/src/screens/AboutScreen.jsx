import React from 'react';
import {View, Text, StyleSheet} from 'react-native';

const AboutScreen = () => {
    return (
        <View style={styles.center}>
            <Text>Лучшее приложение для личных заметок</Text>
            <Text>Версия приложения <Text style={styles.bold}>1.0.0</Text></Text>
        </View>
    );
}

const styles = StyleSheet.create({
    center:{
        flex: 1,
        alignItems: "center",
        justifyContent: "center"
    },
    bold:{
        fontFamily: "bold"
    }
})

export default AboutScreen;
