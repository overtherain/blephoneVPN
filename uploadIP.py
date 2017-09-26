# -*- coding: utf-8 -*-
import os
os.environ['NLS_LANG'] = 'SIMPLIFIED CHINESE_CHINA.UTF8'
import sys
reload(sys)
sys.setdefaultencoding('utf-8')
import requests
import json
import MySQLdb
import logging
import logging.handlers
import time
import datetime
import threading

'''
url='https://121.43.183.196/user/ip.get'
cont=requests.get(url,verify=False).content
out=json.loads(cont)
print out['ip']
'''
script_path = '/home/updateip/'

class Handle:
    def __init_db(self):
        self._mysql_db = MySQLdb.connect(host="121.43.183.196",user="vpn",passwd="vpn@162534",port=3306,db="vpn",charset="utf8")
        self.mysql_cur=self._mysql_db.cursor()
        self.seq = 0
        
    def __init__(self):
        self.__init_db()
        
    def _release_db(self):
        self.mysql_cur.close()
        self._mysql_db.close()
        
    def _do(self):
        url='https://121.43.183.196/user/ip.get'
        cont=requests.get(url,verify=False).content
        out=json.loads(cont)
        now_time = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        logging.debug("ajax request is '%s'.", out['ip'])
        sql_cmd = 'UPDATE vpn_address SET ip = "'+out['ip']+'", updatetime = "'+now_time+'" WHERE id = 1'
        logging.debug("run command is '%s'.", sql_cmd)
        self.mysql_cur.execute(sql_cmd)
        lines = self.mysql_cur.fetchall()
        self._mysql_db.commit()
        if len(lines)==0:
            logging.error("run sql result is null.")

def init_logging():
    fmt = '%(asctime)s[level-%(levelname)s][line:%(lineno)d]:%(message)s'
    logname = script_path + '/log/autoRun'
    logging.basicConfig(level = logging.DEBUG, format='%(asctime)s[level-%(levelname)s][%(name)s]:%(message)s')
    fileshandle = logging.handlers.TimedRotatingFileHandler(logname, when='D', interval=1, backupCount=10)
    fileshandle.suffix = "%Y-%m-%d_%H-%M-%S.log"
    fileshandle.setLevel(logging.DEBUG)
    formatter = logging.Formatter(fmt)
    fileshandle.setFormatter(formatter)
    logging.getLogger().addHandler(fileshandle)

def main():
    p = Handle()
    p._do()
    p._release_db()
    
def run():
    while 1 :
        sql_td = threading.Thread(target = main, name = 'updateIP')
        sql_td.start()
        logging.debug("thread start : '%s'.", sql_td.name)
        exit = 1
        while exit:
            if sql_td.is_alive():
                logging.debug("thread '%s' is alive, wait 10 second.", sql_td.name)
                time.sleep(60)
            else:
                logging.debug("thread '%s' is die.", sql_td.name)
                exit = 0
    
if __name__=="__main__":
    init_logging()
    try:
        pid = os.fork()
        if pid > 0:
            sys.exit(0)
    except OSError, e:
        print >>sys.stderr, "fork #1 failed: %d (%s)" % (e.errno, e.strerror)
        sys.exit(1)
    os.chdir("/")
    os.setsid()
    os.umask(0)
    try:
        pid = os.fork()
        if pid > 0:
            #print "Daemon PID %d" % pid
            sys.exit(0)
    except OSError, e:
        print >>sys.stderr, "fork #2 failed: %d (%s)" % (e.errno, e.strerror)
        sys.exit(1)
    run()
        