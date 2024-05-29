from nltk.probability import FreqDist
import pymorphy2
morph = pymorphy2.MorphAnalyzer()

#удаляем информацию об авторе и конце
def preprocess_train_text(text, author):
    # отсекаем последнюю часть
    text = text.split('Конец ознакомительного фрагмента')[0]

    # убираем первую часть с автором и возвращаем чисто текст в виде массива
    author_splitted = [(t, len(t)) for t in text.split(author)]
    text = sorted(author_splitted, key=lambda chunck: chunck[1], reverse=True)[0][0]
    return text

# анализ частоты слов
def check_most_common_words(lemmedTokens, len_dist = 50):
    freqDist = FreqDist(lemmedTokens)
    print(freqDist.most_common(len_dist))

#лемматизируем и удаляем стоп-слова
def lemmed_and_clear_stop_words(words):
    lemmedTokens = []
    for token in words:
        normal_word = morph.parse(token)[0].normal_form
        lemmedTokens.append(normal_word)
        # удаляем стоп слова
        # if normal_word not in stop_words:
        #     lemmedTokens.append(normal_word)
    #дополнительно фильтруем 50 первых слов
    return lemmedTokens

#удаляем короткие слова
def clear_short_words(words, min_length = 2, min_freq = 3):
    freq_dist = FreqDist(words)
    res =[word for word, freq in freq_dist.items() if len(word) > min_length and freq > min_freq]
    print(res)
    return  res
