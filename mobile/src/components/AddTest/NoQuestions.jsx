import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { Banner } from 'react-native-paper';

const NoQuestions = ({visible = true}) => {
    return (
        <Banner
            visible={visible}
            style={
                {
                    margin: 4
                }
            }
            icon={() => (
                <Ionicons name="alert-circle-outline" size={32} color="blue" />
            )}
        >
            Добавьте как минимум один вопрос для создания тестового этапа
        </Banner>
    );
}


export default NoQuestions;
