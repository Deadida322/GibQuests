import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Card, Text } from 'react-native-paper';
import { AnimatedCircularProgress } from 'react-native-circular-progress';
import { THEME } from '../../theme';



const WatchProgress = ({ progress, count }) => {
    
    const leftContent = () => (
        <AnimatedCircularProgress
            size={60}
            width={5}
            fill={Math.round((progress.progress.stage / count)*100)}
            tintColor={THEME.colors.primary}
            backgroundColor={THEME.colors.secondaryContainer}>
            {
                (fill) => (
                    <Text>
                        {`${progress.progress.stage}/${count}`}
                    </Text>
                )
            }
        </AnimatedCircularProgress>)

    return (
        <Card style={styles.card}>
            <Card.Title subtitle={`${progress.user.name} ${progress.user.surname}`} titleStyle={styles.title} subtitleStyle={styles.title} left={leftContent} title={progress.user.login}/>
            <Card.Content style={{flexDirection: "row", alignItems: "center"}}>
                <Text>
                    Текущий этап - 
                </Text>
                <Text variant='titleMedium' style={{color: THEME.colors.primary}}> {progress.progress.stageName}</Text>
            </Card.Content>
            <Card.Content style={{flexDirection: "row", alignItems: "center"}}>
                <Text>
                    Текущее время - 
                </Text>
                <Text variant='titleMedium' style={{color: THEME.colors.primary}}> {progress.progress.currentTime}</Text>
            </Card.Content>
        </Card>
    );
}

const styles = StyleSheet.create({
    title: {
        marginLeft: 20
    },
    card: {
        margin: 4
    }
})

export default WatchProgress;
