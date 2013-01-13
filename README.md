Market Data downloader for IQFeed data feed.

Program can download ticks, intraday (1, 5, 10, 15, 30 and 60 minutes), daily, weekly and monthly data.

Market symbols you can find here
* http://www.iqfeed.net/symbolguide/index.cfm?symbolguide=lookup&displayaction=support&section=guide&web=iqfeed
* http://www.iqfeed.net/symbolguide/index.cfm?symbolguide=guide&displayaction=support&section=guide&web=iqfeed

Some of them are in Data solution folder.

Output format:
* "Tick Days": tickId , tradetype, year, month, day, time,  last, lastsize, bid,  ask, bidsize, asksize
* "Tick Interval": tickId, tradetype, year, month, day, time, last, lastsize, bid, ask, bidsize, asksize 
* "Intraday Days": year, month, day, time, open, high, low, close, volume
* "Intraday Interval": year, month, day, time, open, high, low, close, volume
* "Daily Days": year, month, day, open, high, low, close , _volume, _openinterest 
* "Daily Interval": year, month, day, open, high, low, close, volume, _openinterest 
* "Weekly": year, month, day, open, high, low, close, volume, openinterest 
* "Monthly": year, month, day, open, high, low, close, volume, openinterest 

IQFeed capabilities:
* 120 calendar days of tick (includes pre-post market).
* Several years of 1-Minute history (Forex back to Feb 2005, Eminis back to Sept. 2005, Stock/Futures/Indexes  back to May 2007) retrieval for charting and time & sales data.
* Daily, Weekly and Monthly Historical data (15+ years of O,H,L,C,V,OI data).
* Daily data for most indexes goes back further than the 15 years.
* Stock and Option Quotes - Real-time or delayed quotes from the Nasdaq, NYSE, AMEX, Canadian and all equity option exchanges.
* Futures, Futures Options and Futures Spreads Quotes - Real-time or delayed quotes from the CBOT, CME, NYMEX, COMEX, NYBOT, KCBT, WPG, MGE, LIFFE, LME, IPE and SGX exchanges.
* European Futures Quotes - Real-time or delayed quotes from the Eurex and Euronext exchanges.  
* Single Stock Futures Quotes - Real-time or delayed quotes from the OneChicago and NQLX exchanges.
