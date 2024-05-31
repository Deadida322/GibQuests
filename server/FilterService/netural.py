import numpy as np
import random as rn

import tensorflow as tf
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split
from keras_preprocessing.text import Tokenizer
from keras_preprocessing.sequence import pad_sequences
import tensorflow.keras.layers as L
from tensorflow.keras.losses import SparseCategoricalCrossentropy

from reader import get_texts

textsWithCategory = get_texts(300)
print('Получили тексты')
texts = [text for text, category in textsWithCategory]
labels = [category for text, category in textsWithCategory]
X_train, X_test, y_train, y_test = train_test_split(texts, labels, test_size=0.1, random_state=42)


vocab_size = 80000

tokenizer = Tokenizer(lower=False, num_words=vocab_size)
tokenizer.fit_on_texts(X_train)

X_train_enc = tokenizer.texts_to_sequences(X_train)
X_test_enc = tokenizer.texts_to_sequences(X_test)

#vocab_size = len(tokenizer.word_index)+1
#exp_sen = 1

print("Vocabulary size: {}".format(vocab_size))

max_len = 10000
X_train_pd = pad_sequences(X_train_enc, padding='post', maxlen=max_len)
X_test_pd = pad_sequences(X_test_enc, padding='post', maxlen=max_len)

# tpu = tf.distribute.cluster_resolver.TPUClusterResolver()
# tf.config.experimental_connect_to_cluster(tpu)
# tf.tpu.experimental.initialize_tpu_system(tpu)
# tpu_strategy = tf.distribute.experimental.TPUStrategy(tpu)
seed_value = 1337
np.random.seed(seed_value)
tf.random.set_seed(seed_value)
rn.seed(seed_value)

# hyper parameters
EPOCHS = 5
BATCH_SIZE = 256
embedding_dim = 16

# with tpu_strategy.scope():
model = tf.keras.Sequential([
    L.Embedding(vocab_size, embedding_dim),
    L.Bidirectional(L.LSTM(64,return_sequences=True)),
    L.Conv1D(64,8),
    L.MaxPool1D(),
    L.Bidirectional(L.LSTM(64,return_sequences=True)),
    L.Conv1D(64,6),
    L.MaxPool1D(),
    L.Bidirectional(L.LSTM(64,return_sequences=True)),
    L.Conv1D(64,3),
    L.MaxPool1D(),
    #L.LSTM(64,return_sequences=True),
    #L.Conv1D(64,2),
    #L.GlobalMaxPooling1D(),
    L.Flatten(),
    L.Dropout(0.5),
    L.Dense(128, activation="relu"),
    L.Dropout(0.5),
    L.Dense(64, activation="relu"),
    L.Dropout(0.5),
    L.Dense(3, activation="softmax")
])


model.compile(loss=SparseCategoricalCrossentropy(),
              optimizer='adam',metrics=['accuracy']
             )

model.summary()

history = model.fit(X_train_pd, y_train, epochs=EPOCHS, validation_split=0.12, batch_size=BATCH_SIZE)

pred = model.predict_classes(X_test_pd[0:1000], batch_size=8)

print('Accuracy: {}'.format(accuracy_score(pred, y_test)))