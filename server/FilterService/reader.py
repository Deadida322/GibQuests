import codecs
import pandas as pd
from dotenv import load_dotenv
import os

from preprocessing import preprocess_train_text, lemmed_and_clear_stop_words, clear_short_words, check_most_common_words

load_dotenv()
directory = os.getenv('DARA_DIR')
base_dir = os.getenv('BASE_DIR')
toxic_dir = os.getenv('TOXIC_DIR')
news_dir = os.getenv('NEWS_DIR')
export_dir = 'process-data-non-freq'
def get_texts():
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    # for row in data[['file', 'rating']].head(1000).itertuples():
    for row in data[['file', 'rating']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "utf-8")
            # fileObj = codecs.open(file_path, "r", "cp1251")
            text = fileObj.read()
            rating = row.rating
            if (rating == 5 or rating == 3 or rating == 8):
                rating = 6
            if(rating == 11 or rating == 12):
                rating = 12
            previews.append((text, rating))
            fileObj.close()
    return previews

def get_texts_2(text_len = 3000):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].head(text_len).itertuples():
    # for row in data[['file', 'rating']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "utf-8")
            # fileObj = codecs.open(file_path, "r", "cp1251")
            text = fileObj.read()
            rating = row.rating
            if (rating == 5 or rating == 3 or rating == 8):
                rating = 6
            if(rating == 11 or rating == 12):
                rating = 12
            previews.append((text, rating))
            fileObj.close()
    return previews

def write_file():
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])
    for row in data[['file', 'author', 'name']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        export_file_path = os.path.join(export_dir, 'previews', row.file)
        if(not os.path.isfile(export_file_path)):
            file_obj = codecs.open(file_path, "r", "utf_8_sig")
            text = file_obj.read()
            author = row.author
            name = row.name

            text = preprocess_train_text(text, author, name)
            lemmed_words = lemmed_and_clear_stop_words(text)

            # not_short_words = clear_short_words(lemmed_words, min_freq=1)

            with open(export_file_path, 'w') as file:
                # дополнительно убираем первый 100 слов
                text_to_write = ' '.join(lemmed_words[100:])
                file.write(text_to_write)
            file_obj.close()
            print(row.file, 'write')

def get_texts_3(skip = 0, take = 0):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].iloc[skip:take].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "cp1251")
            text = fileObj.read()
            rating = row.rating
            previews.append((text, rating))
            fileObj.close()
    return previews

def write_toxic():
    data = pd.read_csv(os.path.join(str(toxic_dir), 'labeled.csv'), delimiter=',')
    i = 0
    for row in data.itertuples():
        filename = str(i) + ".txt"
        export_file_path = os.path.join(toxic_dir, 'texts', filename)
        if(not os.path.isfile(export_file_path) and row.toxic == 1):
            text = row.comment
            lemmed_words = lemmed_and_clear_stop_words(text)

            with open(export_file_path, 'w') as file:
                text_to_write = ' '.join(lemmed_words)
                file.write(text_to_write)
            i += 1

def get_texts_with_toxic():
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "utf-8")
            text = fileObj.read()
            rating = row.rating
            if (rating == 5 or rating == 3 or rating == 8):
                rating = 6
            if(rating == 11 or rating == 12):
                rating = 12
            previews.append((text, rating))
            fileObj.close()

    toxic_file_path = os.path.join(toxic_dir, 'texts')
    toxic_files = [f for f in os.listdir(toxic_file_path) if os.path.isfile(os.path.join(toxic_file_path, f))]
    for tixic_file in toxic_files:
        file_path = os.path.join(toxic_file_path, tixic_file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "utf-8")
            text = fileObj.read()
            rating = 18
            previews.append((text, rating))
            fileObj.close()
    return previews


def write_news():
    data = pd.read_csv(os.path.join(news_dir, 'lenta.csv'), delimiter=',')
    df = pd.DataFrame(data)
    # 'Россия' 'Культура' 'Силовые структуры' 'Мир' 'Интернет и СМИ' 'Спорт'
    #  'Путешествия' 'Наука и техника' 'Бывший СССР' 'Из жизни' 'Ценности'
    #  'Экономика' 'Нацпроекты' 'Среда обитания' 'Дом' '69-я параллель' nan
    #  'Оружие' 'Бизнес' 'Моя страна' 'Мотор' 'Забота о себе'
    filtered_df = df[(df['topic'] == '69-я параллель') | (df['topic'] == 'Нацпроекты') | (df['topic'] == 'Дом') | (df['topic'] == 'Забота о себе')]

    for row in filtered_df[['title', 'text']].itertuples():
        title = row.title
        text = row.text
        filename = title + ".txt"
        export_file_path = os.path.join(news_dir, 'texts', filename)

        if (not os.path.isfile(export_file_path)):
            lemmed_words = lemmed_and_clear_stop_words(text)
            with open(export_file_path, 'w') as file:
                text_to_write = ' '.join(lemmed_words)
                file.write(text_to_write)



def get_news():
    data = pd.read_csv(os.path.join(news_dir, 'lenta.csv'), delimiter=',')
    df = pd.DataFrame(data)
    # 'Россия' 'Культура' 'Силовые структуры' 'Мир' 'Интернет и СМИ' 'Спорт'
    #  'Путешествия' 'Наука и техника' 'Бывший СССР' 'Из жизни' 'Ценности'
    #  'Экономика' 'Нацпроекты' 'Среда обитания' 'Дом' '69-я параллель' nan
    #  'Оружие' 'Бизнес' 'Моя страна' 'Мотор' 'Забота о себе'
    filtered_df = df[(df['topic'] == '69-я параллель') | (df['topic'] == 'Нацпроекты') | (df['topic'] == 'Дом') | (df['topic'] == 'Забота о себе')]
    previews = []
    for row in filtered_df[['title', 'topic']].itertuples():
        title = row.title
        filename = title + ".txt"
        file_path = os.path.join(news_dir, 'texts', filename)

        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "utf-8")
            text = fileObj.read()
            topic = row.topic

            previews.append((text, topic))
            fileObj.close()
    return previews

