from metrics import print_metrics
from reader import get_texts
from sklearn.model_selection import train_test_split
from sklearn.naive_bayes import MultinomialNB
import pickle
import time

from vectorizers import tfidf_vectorizer, countVectorizer

start_time = time.time()

textsWithCategory = get_texts()
print('Получили тексты')
texts = [text for text, category in textsWithCategory]
labels = [category for text, category in textsWithCategory]
X_train, X_test, y_train, y_test = train_test_split(texts, labels, test_size=0.05, random_state=42)

X_train_vectorized = countVectorizer.fit_transform(X_train)
X_test_vectorized = countVectorizer.transform(X_test)

# Обучение классификатора
classifier = MultinomialNB()
classifier.fit(X_train_vectorized, y_train)

predict = classifier.predict(X_test_vectorized)

# точность
print_metrics(y_test, predict)

print("--- %s seconds ---" % (time.time() - start_time))