from reader import get_texts
from sklearn.model_selection import train_test_split
from sklearn.metrics import (accuracy_score, mean_absolute_error as mae,
                             mean_absolute_percentage_error as mape,
                             mean_squared_error as mse,
                             root_mean_squared_error as rmse)
from sklearn.naive_bayes import MultinomialNB
import pickle
from utils import vectorizer
import time

start_time = time.time()

textsWithCategory = get_texts(3000)
texts = [text for text, category in textsWithCategory]
labels = [category for text, category in textsWithCategory]
X_train, X_test, y_train, y_test = train_test_split(texts, labels, test_size=0.1, random_state=42)


X_train_vectorized = vectorizer.fit_transform(X_train)
X_test_vectorized = vectorizer.transform(X_test)

# Обучение классификатора
classifier = MultinomialNB()
classifier.fit(X_train_vectorized, y_train)

# with open('model.pkl','wb') as f:
#     pickle.dump(classifier, f)
    # print('dump')

# print("--- %s seconds ---" % (time.time() - start_time))

# with open('model.pkl', 'rb') as f:
#     classifier = pickle.load(f)
# # Предсказание
predict = classifier.predict(X_test_vectorized)

# точность
print('accuracy_score', accuracy_score(y_test, predict))
print('mae: ', mae(y_test, predict))
print('mape: ', mape(y_test, predict))
print('mse: ', mse(y_test, predict))
print('rmse: ', rmse(y_test, predict))
