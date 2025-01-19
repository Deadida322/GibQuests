import numpy as np
import pandas as pd
import random as rn
import math

from sklearn.model_selection import train_test_split
from keras_preprocessing.text import Tokenizer
from keras_preprocessing.sequence import pad_sequences
import tensorflow.keras.layers as L
from sklearn.preprocessing import LabelEncoder
from tensorflow.keras.losses import SparseCategoricalCrossentropy
import tensorflow as tf

from reader import get_texts, get_texts_3, write_model_params, read_model_params
import time
import os

# hyper parameters
EPOCHS = 5
BATCH_SIZE = 256
embedding_dim = 16

def create_model(vocab_size = 20000):
    model = tf.keras.Sequential([
        L.Embedding(vocab_size, embedding_dim),
        L.Bidirectional(L.LSTM(64, return_sequences=True)),
        L.Conv1D(64, 8),
        L.MaxPool1D(),
        L.Bidirectional(L.LSTM(64, return_sequences=True)),
        L.Conv1D(64, 6),
        L.MaxPool1D(),
        L.Bidirectional(L.LSTM(64, return_sequences=True)),
        L.Conv1D(64, 3),
        L.MaxPool1D(),
        # L.LSTM(64,return_sequences=True),
        # L.Conv1D(64,2),
        # L.GlobalMaxPooling1D(),
        L.Flatten(),
        L.Dropout(0.5),
        L.Dense(128, activation="relu"),
        L.Dropout(0.5),
        L.Dense(64, activation="relu"),
        L.Dropout(0.5),
        L.Dense(4, activation="softmax")
    ])

    model.compile(loss=SparseCategoricalCrossentropy(),
                  optimizer='adam', metrics=['accuracy']
                  )
    return model

def train():
    # take_step = 20
    # skip = read_model_params()
    # write_model_params(20)

    # textsWithCategory = get_texts_3(0, 20)
    textsWithCategory = get_texts()
    start_time = time.time()
    # textsWithCategory = get_texts()
    print('Получили тексты')


    texts = [text for text, category in textsWithCategory]
    labels = [category for text, category in textsWithCategory]
    # кодируем метки
    encoder = LabelEncoder()
    encoder.fit(labels)
    encoded_y = encoder.transform(labels)
    # categorical_labels = to_categorical(encoded_y, num_classes=4)

    X_train, X_test, y_train, y_test = train_test_split(texts, encoded_y, test_size=0.05, random_state=42)

    vocab_size = 20000

    tokenizer = Tokenizer(lower=False, num_words=vocab_size)
    tokenizer.fit_on_texts(X_train)

    X_train_enc = tokenizer.texts_to_sequences(X_train)
    X_test_enc = tokenizer.texts_to_sequences(X_test)

    vocab_size = len(tokenizer.word_index)+1
    #exp_sen = 1

    print("Vocabulary size: {}".format(vocab_size))

    max_len = 10000
    X_train_pd = pad_sequences(X_train_enc, padding='post', maxlen=max_len)
    X_test_pd = pad_sequences(X_test_enc, padding='post', maxlen=max_len)

    seed_value = 1337
    np.random.seed(seed_value)
    tf.random.set_seed(seed_value)
    rn.seed(seed_value)


    model = create_model(vocab_size)
    #display model
    # model.summary()

    checkpoint_path = "training_1/cp-{epoch:04d}.weights.h5"
    checkpoint_dir = os.path.dirname(checkpoint_path)

    # Calculate the number of batches per epoch

    n_batches = len(X_train) / BATCH_SIZE
    n_batches = math.ceil(n_batches)  # round up the number of batches to the nearest whole integer

    # Create a callback that saves the model's weights every 5 epochs
    cp_callback = tf.keras.callbacks.ModelCheckpoint(
        filepath=checkpoint_path,
        verbose=1,
        save_weights_only=True,
        save_freq=5)
    print(n_batches)

    history = model.fit(X_train_pd, y_train, epochs=EPOCHS, batch_size=BATCH_SIZE, callbacks=[cp_callback],
                        validation_data=(X_test_pd, y_test))
    model.save('model.   k eras')

    loaded_model = tf.keras.models.load_model("model.keras")
    loaded_model.summary()
    print("--- %s seconds ---" % (time.time() - start_time))

def get_rating(text):
    loaded_model = tf.keras.models.load_model("model-3.keras")


# get_rating('Текста тваввима')
train()