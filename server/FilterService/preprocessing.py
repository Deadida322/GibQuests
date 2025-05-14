from nltk.probability import FreqDist
from nltk import word_tokenize
import pymorphy3
morph = pymorphy3.MorphAnalyzer()

import nltk
nltk.download('stopwords')
nltk.download('punkt')
nltk.download('punkt_tab')
from nltk.corpus import stopwords

stop_words_ru = list(set(stopwords.words('russian')))
my_stop_words = list(['ооо', '.', ',', '\'', '(', ')', '-', '«', '»', '?', '!', ':', ';', '—', '–', ']', '[', '...'])
stop_words = stop_words_ru + my_stop_words

#удаляем информацию об авторе и конце
def preprocess_train_text(text, author, name):
    # отсекаем последнюю часть
    text = text.split('Конец ознакомительного фрагмента')[0]

    # убираем первую часть с автором и возвращаем чисто текст в виде массива
    # author_splitted = [(t, len(t)) for t in text.split(author)]
    # text = sorted(author_splitted, key=lambda chunck: chunck[1], reverse=True)[0][0]

    name_splitted = [(t, len(t)) for t in text.split(name)]
    text = sorted(name_splitted, key=lambda chunck: chunck[1], reverse=True)[0][0]
    #убираем на всякий случай первые 200 слов
    return text

# анализ частоты слов
def check_most_common_words(lemmedTokens, len_dist = 50):
    freqDist = FreqDist(lemmedTokens)
    print(freqDist.most_common(len_dist))

#лемматизируем и удаляем стоп-слова
def lemmed_and_clear_stop_words(text):
    words = word_tokenize(text)
    lemmedTokens = []
    for token in words:
        normal_word = morph.parse(token)[0].normal_form
        # удаляем стоп слова
        if normal_word not in stop_words and not normal_word.isnumeric():
            lemmedTokens.append(normal_word)
    #дополнительно фильтруем 50 первых слов
    return lemmedTokens

#удаляем короткие слова
def clear_short_words(words, min_length = 2, min_freq = 3):
    freq_dist = FreqDist(words)
    res =[word for word, freq in freq_dist.items() if len(word) > min_length and freq > min_freq]
    print(res)
    return res
