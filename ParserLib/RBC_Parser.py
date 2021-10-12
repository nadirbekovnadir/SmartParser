from datetime import datetime, timedelta

import requests as req
from bs4 import BeautifulSoup

from copy import deepcopy


class rbc_parser:

  @staticmethod
  def get_items(soup):
    return soup.find_all("div", attrs={ "class": "js-news-feed-item js-yandex-counter"})

  @staticmethod
  def get_title(item):
    return str.strip(item.find('span', attrs={ "class": "item__title rm-cm-item-text"}).text)

  @staticmethod
  def get_description(item):
    return None

  @staticmethod
  def get_link(item):
    return item.find('a', attrs={ "class": "item__link"}).get('href')

  @staticmethod
  def get_published(item):
    result = datetime.today() + timedelta(hours=4)

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

    return result

  @staticmethod
  def parse(url):

    resp = req.get("https://www.rbc.ru/short_news")
    soup = BeautifulSoup(resp.text, 'lxml')

    items = rbc_parser.get_items(soup)

    entries = []
    entry = {}
    for item in items:
      entry['title'] = rbc_parser.get_title(item)
      entry['description'] = rbc_parser.get_description(item)
      entry['link'] = rbc_parser.get_link(item)
      entry['published'] = rbc_parser.get_published(item)

      entries.append(deepcopy(entry))
    
    return {'entries': entries}