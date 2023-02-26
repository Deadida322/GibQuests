import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Card, Text, Menu, Button, IconButton } from "react-native-paper"
import { THEME } from "../theme"

const QuestItem = ({ quest, onQuestPreview, isAdmin = false, onOpen, onWatch, shortInfo = false, onEdit }) => {
    const [showMenu, setShowMenu] = useState(false)
    return (
        <Card style={styles.card}>
            <View style={styles.img}>
                <Card.Cover style={{ marginTop: -8 }} source={{ uri: quest.img }} />
            </View>

            <Card.Title title={quest.title} subtitle={`${quest.stages.length} этапов`} />
            {!shortInfo ?
                <Card.Content>
                    <Text>{quest.description}</Text>
                </Card.Content> : ""
            }
            <Card.Actions>
                {!shortInfo ?
                    <>
                        {!isAdmin ?
                            <Button icon="eye" mode="contained" onPress={onQuestPreview}>Посмотреть</Button>
                            :
                            <>
                                <Menu
                                    anchorPosition='top'
                                    visible={showMenu}
                                    onDismiss={() => setShowMenu(false)}
                                    anchor={<IconButton
                                        icon="dots-vertical"
                                        mode="contained-tonal"
                                        iconColor={THEME.colors.primary}
                                        size={20}
                                        onPress={() => setShowMenu(true)}
                                    />}>
                                    <Menu.Item leadingIcon="pencil-outline" onPress={() => { onEdit(); setShowMenu(false) }} title="Редактировать" />
                                    <Menu.Item leadingIcon="account-eye" onPress={() => { onWatch(); setShowMenu(false) }} title="Отследить" />
                                    <Menu.Item leadingIcon="share-outline" onPress={() => { }} title="Поделиться" />
                                    <Menu.Item leadingIcon="delete-outline" onPress={() => { }} title="Удалить" />

                                </Menu>
                                <Button icon="eye" mode="contained" onPress={onQuestPreview}>Посмотреть</Button>
                            </>
                        }</> : ""}
            </Card.Actions>
        </Card>
    );
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 4,
    },
    img: {
        borderRadius: 4,
        borderBottomEndRadius: 0,
        borderBottomStartRadius: 0,
        height: 150,
        overflow: "hidden"
    }
})

export default QuestItem;
