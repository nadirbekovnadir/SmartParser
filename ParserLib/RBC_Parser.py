from datetime import datetime, timedelta

import requests as req
from bs4 import BeautifulSoup

from copy import deepcopy


class rbc_parser:

    @staticmethod
    def _parse_items(soup):
        result = None
        try:
            result = soup.find_all("div", attrs={ "class": "js-news-feed-item js-yandex-counter"})
        except:
            pass

        return result

    @staticmethod
    def _parse_title(item):
        result = None
        try:
            result = str.strip(item.find('span', attrs={ "class": "item__title rm-cm-item-text"}).text)
        except:
            pass

        return result

    @staticmethod
    def _parse_description(item):
        return None

    @staticmethod
    def _parse_link(item):
        result = None
        try:
            result = item.find('a', attrs={ "class": "item__link"}).get('href')
        except:
            pass

        return result

    @staticmethod
    def _parse_published(item):
        result = datetime.today() + timedelta(hours=4)

        try:
            published = item.find(
                'div', attrs={ "class": "item__bottom"}
                ).find('span', attrs={ "class": "item__category"}).text
    
            date_time = published.split(', ')
            hour_min = date_time[-1].split(sep=':')
            hour = int(hour_min[0])
            minute = int(hour_min[1])
            second = 0

            if len(date_time) > 1:
                day = int(date_time[-2].split(' ')[0])

                result = result.replace(day=day)

            result = result.replace(hour=hour, minute=minute, second=second)
        except:
            pass

        return result


    @staticmethod
    def get_name():
        return "RBC"


    @staticmethod
    def get_link():
        return "https://www.rbc.ru/short_news"


    @staticmethod
    def parse(html):
        soup = BeautifulSoup(html, 'lxml')

        items = rbc_parser._parse_items(soup)

        entries = []
        entry = {}
        for item in items:
            entry['title'] = rbc_parser._parse_title(item)
            entry['description'] = rbc_parser._parse_description(item)
            entry['link'] = rbc_parser._parse_link(item)
            entry['published'] = rbc_parser._parse_published(item)

            entries.append(deepcopy(entry))

        return {'entries': entries}