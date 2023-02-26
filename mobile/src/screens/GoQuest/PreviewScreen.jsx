import React from 'react';
import { View, ScrollView, StyleSheet, ImageBackground } from 'react-native';
import StageItem from '../../components/StageItem';
import { LinearGradient } from "expo"
import { Text, Card, IconButton, Button, Badge } from 'react-native-paper';
import { THEME } from '../../theme';

const PreviewScreen = ({ navigation, route }) => {
    const quest = route.params.quest
    return (
        <ScrollView style={styles.main}>
            <View style={styles.imageWrapper}>
                <ImageBackground style={styles.theImage} source={{ uri: quest.img }}>
                    <View style={styles.imageWrapper}>
                        <Text variant='headlineMedium' style={styles.title}>{quest.title}</Text>
                    </View>
                </ImageBackground>
            </View>
            <Card style={styles.card}>
                <Card.Title title="О квесте" />
                <Card.Content>
                    <Text variant="bodyMedium">{quest.description}</Text>
                </Card.Content>
                <Card.Actions>
                    <Button
                        icon="play-outline"
                        size={20}
                        mode="contained"
                        onPress={() => navigation.navigate("GoQuest", {quest})}
                    >Начать</Button>
                    <IconButton
                        icon="share-outline"
                        mode="contained-tonal"
                        onPress={() => console.log('Pressed')}
                    />
                </Card.Actions>
            </Card>

            <View style={{ ...styles.card, padding: 4 }}>
                <View style={styles.badgeContainer}>
                    <Text variant='titleMedium' style={{ color: THEME.colors.secondary }}>
                        Этапы
                    </Text>
                    <Badge style={styles.badge}>{quest.stages.length}</Badge>
                </View>
                {quest.stages.map(item => (<StageItem editable={false} stage={item}></StageItem>))}
            </View>
        </ScrollView >
    );
}

const styles = StyleSheet.create({
    card: {
        margin: 5,
        borderRadius: 4
    },
    title: {
        marginTop: 6,
        marginBottom: 6,
        color: THEME.colors.onPrimary,
        textShadowOffset: { width: 1, height: 1 },
        textShadowRadius: 1
    },
    imageWrapper: {
        borderTopLeftRadius: 0,
        borderTopRightRadius: 0,
        height: 200,
        width: "100%",
        justifyContent: 'center',
        alignItems: 'center',
        overflow: "hidden",
        backgroundColor: 'rgba(0,0,0,0.3)',
        borderRadius: 8
    },
    theImage: {
        width: "100%",
        height: "100%",
        resizeMode: "cover",
        justifyContent: 'center',
        alignItems: 'center',
    },
    badgeContainer: {
        position: "relative"
    },
    badge: {
        position: "absolute",
        top: 3,
        right: 3
    }
})

export default PreviewScreen;
