import React from 'react';
import {View, StyleSheet} from 'react-native';
import { Ionicons } from '@expo/vector-icons';

import {
    HeaderButtons,
    HeaderButton,
    Item,
    HiddenItem,
    OverflowMenu,
  } from 'react-navigation-header-buttons';
import { Entypo } from '@expo/vector-icons'; 


const AppHeaderIcon = (props) => {
    return (
        <HeaderButton IconComponent={Ionicons} iconSize={24} {...props} color="white"/>
    );
}

const styles = StyleSheet.create({})

export default AppHeaderIcon;
