import * as SQLite from 'expo-sqlite'

const db = SQLite.openDatabase('post.db')

export class DB {
    static init(){
        console.log('init')
        return new Promise((resolve, reject)=>{
            db.transaction(tx=>{
                tx.executeSql(
                    'CREATE TABLE IF NOT EXISTS posts (id INTEGER PRIMARY KEY NOT NULL, text TEXT NOT NULL, img TEXT, date TEXT, booked INT)',
                    // 'DROP TABLE posts',
                    [],
                    resolve,
                    (_, err)=>{console.log(err)}
                )
            })
        })
    }
    static getPosts(){
        return new Promise((resolve, reject)=>{
            db.transaction(tx=>{
                tx.executeSql(
                    'SELECT * FROM posts',
                    [],
                    (_, res) => resolve(res.rows._array),
                    (_, err)=>{reject(err)}
                )
            })
        })
    }
    static createPost(post){
        return new Promise((resolve, reject)=>{
            db.transaction(tx=>{
                tx.executeSql(
                    `INSERT INTO posts (text, date, booked, img) VALUES (?, ?, ?, ?)`,
                    [post.text, post.date, post.booked, post.img],
                    (_, res)=>{resolve(res.insertId)},
                    (_, err)=>reject(err)
                )
            })
        })
    }
    static removePost(id){
        console.log('remove called')
        return new Promise((resolve, reject)=>{
            db.transaction(tx=>{
                tx.executeSql(
                    `DELETE FROM posts WHERE id = ?`,
                    [id],
                    (_, res)=>{resolve(res)},
                    (_, err)=>reject(err)
                )
            })
        })
    }
    static  updatePost(post){
        return new Promise((resolve, reject)=>{
            db.transaction(tx=>{
                tx.executeSql(
                    `UPDATE posts SET booked = ? WHERE id = ?`,
                    [post.booked ? 0 : 1, post.id],
                    (_, res)=>{resolve()},
                    (_, err)=>reject(err)
                )
            })
        })
    }
}