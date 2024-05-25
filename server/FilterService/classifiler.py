from reader import get_texts
from nltk.corpus import stopwords
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics import accuracy_score
from sklearn.naive_bayes import MultinomialNB

stop_words = list(set(stopwords.words('russian')))

textsWithCategory = get_texts(3000)
texts = [text for text, category in textsWithCategory]
labels = [category for text, category in textsWithCategory]
X_train, X_test, y_train, y_test = train_test_split(texts, labels, test_size=0.1, random_state=42)
vectorizer = CountVectorizer(stop_words=stop_words, ngram_range=(1, 1))

X_train_vectorized = vectorizer.fit_transform(X_train)
X_test_vectorized = vectorizer.transform(X_test)

# Обучение классификатора
classifier = MultinomialNB()
classifier.fit(X_train_vectorized, y_train)

# Предсказание
predict = classifier.predict(X_test_vectorized)

# Возращаем точность
print(accuracy_score(y_test, predict))
