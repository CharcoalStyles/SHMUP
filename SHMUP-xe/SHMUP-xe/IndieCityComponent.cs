/******************************************************************************************
IndieCityComponent - a XNA GameComponent for the integration with the IndieCity ICELib,
                     with support for achievements and leaderboards.

-------------------------------------------------------------------------------------------

Copyright (c) 2011 Spyn Doctor Games (Johannes Hubert). All rights reserved.

Redistribution and use in binary forms, with or without modification, and for whatever
purpose (including commercial) are permitted. Attribution is not required. If you want
to give attribution, use the following text and URL (may be translated where required):
		Uses source code by Spyn Doctor Games - http://www.spyn-doctor.de

Redistribution and use in source forms, with or without modification, are permitted
provided that redistributions of source code retain the above copyright notice, this
list of conditions and the following disclaimer.

THIS SOFTWARE IS PROVIDED BY SPYN DOCTOR GAMES (JOHANNES HUBERT) "AS IS" AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
SPYN DOCTOR GAMES (JOHANNES HUBERT) OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Last change: 2011-11-27

Developed with ICELib version 1.0.1.5336.

Compatible with XNA 3.1, XNA 4.0 and .NET 2.0 (or later).

-------------------------------------------------------------------------------------------

The goal was to create a GameComponent that is easy to integrate into a XNA game and that
does all the session updates, state management and event handling for you, so that you
don't have to deal with them yourself.


Disclaimer: This component was written against a beta version of the ICELib, for which the
documentation at this time is rather incomplete. Therefore, if something in the code seems
strange to you, then this may be because I was forced to work around some peculiar behavior
in the current ICELib (of course it may also be because I just didn't understand the ICELib
properly ;-).
Some of the code is also untested. For example the methods for paging a leaderboard are
kind of hard to test if the only leaderboards I can test with are either empty or have only
a single entry (i.e. my own). Similarly, some methods are untested because the underlying
method in the ICELib itself is still not implemented (for example "RequestLastPage").
The component will likely change a bit in future versions, as the ICELib matures.



-------------------------------------------------------------------------------------------
Integrating the Component With Your Game
----------------------------------------

This is very simple. First some preparation:

- Install the ICSDK (make sure to get the latest version).

- Add ICEBridge, ICECore and ICELanda to the "References" section of your game
  (you find them on the "COM" tab of the "Add Reference" dialog).

- Add the IndieCityComponent.cs file to your project.

- Edit the namespace in that file to match your project.
 
- Make sure to run icdevreg2 (from the ICSDK) and register your game and your IndieCity
  username. Also generate an access token for the game under your username.


Then in your main Game class, add a member for the component. Let's assume that you called
this member "mIndieCityComponent". Then add the following to the Initialize method of your
Game class:

	mIndieCityComponent = new IndieCityComponent(
		"11111111-2222-3333-4444-555555555555",	// replace with your Game ID
		"33333333-4444-5555-6666-777777777777",	// replace with your ICELib ID
		"66666666-7777-8888-9999-000000000000", // replace with your ICELib Secret
		true,	// replace with false if your game has no achievements
		true,	// replace with false if your game has no leaderboards
		this);
	Components.Add(mIndieCityComponent);

That's it. :-)



-------------------------------------------------------------------------------------------
IndieCity Component Properties
------------------------------

The component exposes the following properties:

	SessionActive: "true", if the IndieCity session is connected, "false" if not.

	UserId: The IndieCity ID of the current user.

	UserName: The IndieCity user name of the current user.

	SessionEndDelegate: A delegate that is called after the IndieCity session
						ends for some reason (see below for details).



-------------------------------------------------------------------------------------------
Starting an IndieCity Session
-----------------------------

Before you can do anything with the component, you need to start an IndieCity session. So
at a convenient location in your code, add the following:

	mIndieCityComponent.RequestSessionStart(null);

For example, you could do this after loading the game assets, or at a similar moment.

Note, that this call only begins the process of starting a session. The session will not be
completely started immediately, but will take some time to become active.

While the session is not yet active (or hasn't even been started yet), any calls to the
achievement or leaderboard related methods will have no effect and will return empty
defaults (except UnlockAchievement and PostLeaderboardScore, which work even while no
session is currently active, see below for details).

To see if the session is active already, you can check the SessionActive property.

Optionally, if you want to be notified as soon as the session becomes active, you can also
provide a callback delegate to the RequestSessionStart call (instead of the "null"
argument). For example with an in-line delegate:

	mIndieCityComponent.RequestSessionStart(delegate()
	{
		// This code will be called once the session has been
		// started and is active.	
	});

Of course you can also specify any other method as a delegate that matches the
ICCNotificationDelegate signature.



-------------------------------------------------------------------------------------------
Ending an IndieCity Session
---------------------------

Normally, you will probably never need to end the IndieCity session yourself, because the
component already ends the session for you when the game exits.

But if for some reason you want to prematurely end the session anyway, you can call:

	mIndieCityComponent.EndSession();


While you will usually not need to end the session yourself, you need to be aware that an
IndieCity session can also end for some other reason than you ending it actively. For
example it could end because there is a network timeout.

Once a session ends, the SessionActive property will be "false" again.

If you want to know the reason *why* the session was ended, and/or if you want to be
informed as soon as the session ends (and not only when you check the SessionActive
property the next time), you can use the SessionEndDelegate:

After you create your mIndieCityComponent, and before starting the session, set the
SessionEndDelegate property to point to a callback method that is to be called when the
session ends. For example with an in-line delegate:

	mIndieCityComponent.SessionEndDelegate = delegate(IndieCityComponent.SessionEndReason reason)
	{
		// This code will be called once the session has ended,
		// with the applicable reason
	}

(Or any other delegate method that matches the ICCSessionEndDelegate signature.)


Another example - light-weight DRM:

If the game is an unlicensed (for example pirated) copy of the game, i.e. if
CoAccessControl.LicenseState == LicenseState.LS_NOLICENSE, then after the session starts,
the component will immediately end the session again, with the SessionEndReason.NO_LICENSE.
That way, in an unlicensed game the component will never be active, so achievements and
leaderboards will never work.

Additionally, you can use this as a light-weight DRM that shuts down the game if it is
unlicensed, like this:

	mIndieCityComponent.SessionEndDelegate = delegate(IndieCityComponent.SessionEndReason reason)
	{
		if (reason == IndieCityComponent.SessionEndReason.NO_LICENSE ||
		    reason == IndieCityComponent.SessionEndReason.BAD_CREDENTIALS)
		{
			Exit(); // replace with the correct code to exit the game
		}
	}

This callback will automatically close the game if the session ends either because there is
no license or because of incorrect user credentials. If however there simply is no internet
connection to IndieCity, the game will still run (but of course then you can't access
IndieCity achievements or leaderboards).



-------------------------------------------------------------------------------------------
Indie City Achievements
-----------------------

If you have created a valid achievement set on the IndieCity developers site and have
associated it with your game, you can pass "true" for the "hasAchievements" parameter of
the component's constructor and then access the achievements through the component.

In this case, when you request to start a session, the component will automatically
download all achievement data as part of the session start process.

After the session is started (i.e. SessionActive == true), you can access the achievement
related methods to get access to the achievements:

void UnlockAchievement(long achievementID) :
Unlocks the achievement with the given ID for the current user in the IndieCity achievement
database and stores the same information in the locally cached achievement data.
NOTE: Theoretically you can also unlock an achievement by calling the CoAchievement.Unlock()
method directly, but if you do this, you bypass the component's local achievement data cache.
So while the achievement will then correctly be marked as unlocked in the IndieCity
achievement database, any call to "IsAchievementUnlocked" (see below) will still return
"false" (until you refresh the achievement data again, see below). It is therefore STRONGLY
recommended to only unlock achievements through this method!
SPECIAL NOTE: You can call this method even if the session is not active (SessionActive == false).
In this case, the unlocked achievement will be stored locally by the ICELib and will
automatically be transferred to the IndieCity achievement database the next time a session
is established.

int GetAchievementCount() :
Returns the number of achievements in the achievement set in the IndieCity database.
NOTE: This method accesses the component's cached achievement data that was loaded when the
session was started (or during the last call to RequestAchievementDataRefresh, see below).
It does not necessarily reflect the current information in the IndieCity achievement database,
if the achievement set has been changed in the meantime.

CoAchievement GetAchievementByID(long achievementID) :
Returns the achievement with the given ID (the ID is defined on the IndieCity developers
site). You can then access the achievement details via the returned object.
NOTE: Like above, this method returns information from the component's cached achievement data!

CoAchievement GetAchievementByIndex(int index) :
Returns the achievement with the given index (where 0 <= index < achivement_count). You can
then access the achievement details via the returned object.
NOTE: Like above, this method returns information from the component's cached achievement data!

bool IsAchievementUnlocked(long achievementID) :
"true" if in the IndieCity achievement database the achievement with the given ID is
unlocked for the current user, "false" otherwise.
NOTE: Like above, this method returns information from the component's cached achievement data!
This means in particular, that if an achievement has become unlocked in the IndieCity
achievement database in the meantime (through a channel other than the UnlockAchievement
method, see above), then the value returned by this method may be incorrect!

void RequestAchievementDataRefresh(ICCNotificationDelegate refreshCompleteDelegate) :
Normally you will not have any need to call this method, because all achievement data is
already downloaded from the IndieCity server into the component's local cache while the
component starts the session. This includes both the information about the achievements
themselves, and the information which of them are unlocked for the current user. So once
the session is active, you can access this cached data via the methods described above.
However, in some situations you may want to refresh the cached data while the session is
already active (i.e. while the game is already running). For example:
If your in-game achievement UI displays the achievement score, and you want to display the
"true-score" for an achievement. This true-score can change over time, as more and more
players unlock achievements of the game. So when the user views the in-game achievement UI,
the true-score that was cached when the session was started (usually at game start) may
already be outdated. If you want to make sure that your UI displays the latest true-score,
you can use this method to refresh the achievement cache. Of course if you don't display
the true-score in the first place, you don't have to care about this.
If you call this refresh method, then the same data refresh procedure is triggered that is
also performed during a session start. The refreshed achievement data is not available
immediately after the call returns, but takes a while to be fetched from the server. So
normally you will want to supply a delegate for the "refreshCompleteDelegate" parameter,
which is called as soon as the achievement refresh has finished.
NOTE: Do not call this method a second time while the previous request is still pending,
i.e. do not call it again before the "refreshCompleteDelegate" has been called back.


IMPORTANT: Do not call any of the achievement related methods if you have passed "false"
for the "hasAchievements" argument in the constructor. If you do, the behavior of the
methods is undefined and may cause exceptions or otherwise unexpected behavior.



-------------------------------------------------------------------------------------------
Indie City Leaderboards
-----------------------

If you have created a valid leaderboard set on the IndieCity developers site and have
associated it with your game, you can pass "true" for the "hasLeaderboards" parameter of
the component's constructor and then access the leaderboards through the component.

After the session is started (i.e. SessionActive == true), you can access the leaderboard
related methods to get access to the leaderboards:

void PostLeaderboardScore(int leaderboardID, long score)
Posts an entry with the given score and for the current user to the leaderboard with the
given ID in the IndieCity leaderboard database (the ID is defined on the IndieCity
developers site).
SPECIAL NOTE: You can call this method even if the session is not active (SessionActive == false).
In this case, the posted score will be stored locally by the ICELib and will automatically
be transferred to the IndieCity leaderboard database the next time a session is established.

CoLeaderboardPage RequestOpenLeaderboard(int leaderboardID, int pageSize,
	ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Request the first page of the leaderboard with the given ID. The page will have the given
page size. The page object is returned immediately, but will not yet be completely
populated. This will take some time. So you will have to store a reference to the returned
page and wait until it is populated before you can display it. You also need to hang on to
this page object because you will need it for the other page related methods (see below).
I.e. before you can call any of the other page related methods, you first need to obtain a
page object with this RequestOpenLeaderboard method.
To wait until a page has been populated, you can either poll the
CoLeaderboardPage.PopulationState property yourself (in which case you would usually supply
"null" for the pageCompleteDelegate parameter), or more comfortably, you can supply a
pageCompleteDelegate that will be called as soon as the page is populated. For example with
an in-line delegate:

	mIndieCityComponent.RequestOpenLeaderboard(1, 10, delegate(bool success)
	{
		if (success) {
			// The page has been populated successfully,
			// you can now access its rows.
		}
		else {
			// There was an error during the page population,
			// you should not use the associated page object.
		}
	});

(Or any other delegate method that matches the ICCLeaderboardPageLoadDelegate signature.)


void RequestFirstPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Sets the given page (which must have been obtained with RequestOpenLeaderboard) to point to
the first entries of the leaderboard that the page belongs to. The number of entries (i.e.
the size of the page) depends on the page size that was specified during
RequestOpenLeaderboard. For example for page size 10, this call will request entries 1-10.
Just like with RequestOpenLeaderboard, the page will not immediately be populated with the
requested entries, but this will take some time. So again, you can either poll
CoLeaderboardPage.PopulationState (and supply "null" for the pageCompleteDelegate) to wait
until the page is populated, or you can supply a pageCompleteDelegate and receive a callback
(with a success flag) as soon as the page is populated. See RequestOpenLeaderboard for
details about this delegate.

void RequestLastPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Similar to RequestFirstPage, but sets the page to the last entries of the leaderboard.
NOTE: In version 1.0.1.5336 of the ICELib, this method is still not implemented. You get an
exception if you try to call it!

void RequestNextPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Similar to RequestFirstPage, but sets the page to the next entries of the leaderboard,
depending on the selected page size and the entries that are currently in the page. For
example (with page size 10), if the page currently shows entries 1-10, then this call will
request entries 11-20.

void RequestPrevPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Similar to RequestFirstPage, but sets the page to the previous entries of the leaderboard,
depending on the selected page size and the entries that are currently in the page. For
example (with page size 10), if the page currently shows entries 51-60, then this call will
request entries 41-50.

void RequestUserPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate) :
Similar to RequestFirstPage, but sets the page so that it contains the entry of the current
user, plus as many entries "around" the current user's entry as are necessary to fill up
the page to the current page size.
NOTE: In version 1.0.1.5336 of the ICELib, this method always fills the user page in such
a way, that the current user's entry is in the middle of the page. For example for page
size 10, the current user's entry will end up as the 5th entry of the returned page, no
matter what rank the player has (unless there are not enough entries before the user's entry).
That means, that the returned page will usually not start on a normal page boundary. For
example with page size 10, you would expect pages that start on rank 1, 11, 21, 31, etc.
But RequestUserPage may return a page that starts on any rank. For example with page size 10
and a user rank of 22, the page will start with rank 18, instead of the expected "even" page
boundary rank 21. This means, that if you then use RequestNextPage or RequestPrevPage, that
those will then also return pages that return first ranks that are similarly "odd". So if
after a call to RequestUserPage the current page starts with rank 18 and you now call
RequestNextPage, then the returned page will show ranks 28 - 37. Or if you call RequestPrevPage,
the returned page will show ranks 8 - 17. If you then call RequestPrevPage again, there is
no full page (of size 10) left anymore, since it is not possible to display ranks -2 - 7.
So instead the ICELib returns an incomplete page that contains ranks 1 - 7.

CoLeaderboardUserRows RequestUsersScores(ICCUserScoresLoadDelegate scoresCompleteDelegate)
Requests a list of all leaderboard entries of the current user, in all leaderboards. For
each leaderboard on which the user has an entry, the list will contain that matching entry.
The list is returned in form of a CoLeaderboardUserRows object. This list object is returned
immediately, but will not yet be completely populated. This will take some time. So you
will have to store a reference to the returned list and wait until it is populated before
you can access it.
To wait until the list has been populated, you can either poll the
CoLeaderboardUserRows.PopulationState property yourself (in which case you would usually
supply "null" for the scoresCompleteDelegate parameter), or more comfortably, you can supply
a scoresCompleteDelegate that will be called as soon as the list is populated. For example
with an in-line delegate:

	mIndieCityComponent.RequestUsersScores(delegate(CoLeaderboardUserRows scoreList)
	{
		if (scoreList != null) {
			// The list has been populated successfully,
			// you can now access its entries.
		}
		else {
			// There was an error during the list population,
		}
	});

(Or any other delegate method that matches the ICCUserScoresLoadDelegate signature.)
NOTE: In version 1.0.1.5336 of the ICELib, if you call GetRowFromId(leaderboardID) on the
returned list object for a leaderboard on which the current user does not have an entry,
you will get an Exception. You need to catch this exception to handle the case that a user
does not have an entry on the leaderboard with the given leaderboardID.

NOTE: Do not call any of these leaderboard releated RequestXXX methods while a previous
request is still pending. I.e. do not call any of these RequestXXX methods again if you
already have called the same or one of the other leaderboard related RequestXXX methods
before, and the pageCompleteDelegate of that previous call has not yet been called back,
or the CoLeaderboardPage.PopulationState of the page is still
LeaderboardPopulationState.LPS_PENDING.

IMPORTANT: Do not call any of the leaderboard related methods if you have passed "false"
for the "hasLeaderboards" argument in the constructor. If you do, the behavior of the
methods is undefined and may cause exceptions or otherwise unexpected behavior.
 
*****************************************************************************************

using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Collections.Generic;
using ICEBridgeLib;
using ICECoreLib;
using ICELandaLib;

namespace SHMUP
{
	public delegate void ICCNotificationDelegate();
	public delegate void ICCSessionEndDelegate(IndieCityComponent.SessionEndReason reason);
	public delegate void ICCLeaderboardPageLoadDelegate(bool success);
	public delegate void ICCUserScoresLoadDelegate(CoLeaderboardUserRows scores);

	public class IndieCityComponent : GameComponent, IEventHandler, IAchievementEventHandler, ILeaderboardEventHandler
	{
		private CoGameSession mSession;
		private uint mSessionCookie;
		private SessionStartPhase mSessionStartPhase;
		private ICCNotificationDelegate mSessionStartedDelegate;
		private bool mSessionEndBecauseNoLicense;

		private CoAchievementManager mAchievementManager;
		private uint mAchievementCookie;
		private Dictionary<long, bool> mAchievementsUnlockedAtIndieCity;
		private AchievementRefreshInfo mAchievementRefreshInfo;
		private bool mAchievementsReady;

		private CoLeaderboardManager mLeaderboardManager;
		private uint mLeaderboardCookie;
		private CoLeaderboardPage mCurrentlyLoadingLeaderboardPage;
		private ICCLeaderboardPageLoadDelegate mLeaderboardPageLoadDelegate;
		private CoLeaderboardUserRows mCurrentlyLoadingUserScores;
		private ICCUserScoresLoadDelegate mUserScoresLoadDelegate;
		private bool mLeaderboardsReady;

		private bool mSessionActive;
		public bool SessionActive
		{
			get { return mSessionActive; }
		}

		private ICCSessionEndDelegate mSessionEndDelegate;
		public ICCSessionEndDelegate SessionEndDelegate
		{
			set { mSessionEndDelegate = value; }
		}

		public int UserId
		{
			get { return mSession.UserId; }
		}

		private string mUserName;
		public string UserName
		{
			get { return mUserName; }
		}

		public enum SessionEndReason
		{
			UNKNOWN,
			USER_REQUEST,
			NO_CONNECTION,
			TIMEOUT,
			BAD_CREDENTIALS,
			NO_LICENSE,
		}

		private enum SessionStartPhase
		{
			NOT_STARTED,
			REQUEST_SESSION,
			WAITING_FOR_INDIECITY_DATA,
			ACTIVE
		}

		public IndieCityComponent(string gameID, string serviceID, string serviceSecret, bool hasAchievements, bool hasLeaderboards, Game myGame)
			: base(myGame)
		{
			Enabled = false;

			ServiceId serviceIDStruct = new ServiceId();
			serviceIDStruct.Data1 = uint.Parse(serviceID.Substring(0, 8), NumberStyles.HexNumber);
			serviceIDStruct.Data2 = ushort.Parse(serviceID.Substring(9, 4), NumberStyles.HexNumber);
			serviceIDStruct.Data3 = ushort.Parse(serviceID.Substring(14, 4), NumberStyles.HexNumber);
			serviceIDStruct.Data4 = new byte[] {
				byte.Parse(serviceID.Substring(19, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(21, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(24, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(26, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(28, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(30, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(32, 2), NumberStyles.HexNumber),
				byte.Parse(serviceID.Substring(34, 2), NumberStyles.HexNumber),
			};

			CoBridge2 bridge = new CoBridge2();
			bridge.Initialise(gameID);
			bridge.SetServiceCredentials(GameService.GameService_IndieCityLeaderboardsAndAchievements,
										 serviceIDStruct, serviceSecret);

			mSession = bridge.CreateDefaultGameSession();
			mSessionCookie = mSession.RegisterEventHandler(0, 0, this);
			mSessionActive = false;
			mSessionStartPhase = SessionStartPhase.NOT_STARTED;

			mUserName = bridge.UserStore.GetUserFromId(mSession.UserId).Name;

			mAchievementsReady = !hasAchievements;
			if (hasAchievements) {
				mAchievementManager = new CoAchievementManager();
				mAchievementManager.SetGameSession(mSession);
				mAchievementManager.InitialiseAchievements(null);
				mAchievementCookie = ((IAchievementService)mAchievementManager).RegisterAchievementEventHandler(this);
				mAchievementsUnlockedAtIndieCity = new Dictionary<long, bool>();
			}

			mLeaderboardsReady = !hasLeaderboards;
			if (hasLeaderboards) {
				mLeaderboardManager = new CoLeaderboardManager();
				mLeaderboardManager.SetGameSession(mSession);
				mLeaderboardManager.InitialiseLeaderboards(null);
				mLeaderboardCookie = ((ILeaderboardService)mLeaderboardManager).RegisterLeaderboardEventHandler(this);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (mAchievementManager != null)
					((IAchievementService)mAchievementManager).UnregisterAchievementEventHandler(mAchievementCookie);
				if (mLeaderboardManager != null)
					((ILeaderboardService)mLeaderboardManager).UnregisterLeaderboardEventHandler(mLeaderboardCookie);
				mSession.UnregisterEventHandler(mSessionCookie); // unregister event handler first, so that EndSession below does not cause an event
				if (mSession.IsSessionStarted())
					mSession.EndSession();
			}
			base.Dispose(disposing);
		}

		public void RequestSessionStart(ICCNotificationDelegate startedDelegate)
		{
			if (!Enabled) {
				mSessionStartPhase = SessionStartPhase.REQUEST_SESSION;
				mSessionStartedDelegate = startedDelegate;
				mSession.RequestStartSession();
				Enabled = true;
			}
		}

		public void EndSession()
		{
			if (Enabled)
				mSession.EndSession();
		}

		public override void Update(GameTime gameTime)
		{
			mSession.UpdateSession();

			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null && mCurrentlyLoadingLeaderboardPage.PopulationState != LeaderboardPopulationState.LPS_PENDING) {
					bool success = mCurrentlyLoadingLeaderboardPage.PopulationState == LeaderboardPopulationState.LPS_POPULATED;
					mCurrentlyLoadingLeaderboardPage = null;
					if (mLeaderboardPageLoadDelegate != null)
						mLeaderboardPageLoadDelegate.Invoke(success);
				}
				if (mCurrentlyLoadingUserScores != null && mCurrentlyLoadingUserScores.PopulationState != LeaderboardPopulationState.LPS_PENDING) {
					CoLeaderboardUserRows scores = mCurrentlyLoadingUserScores;
					mCurrentlyLoadingUserScores = null;
					if (mUserScoresLoadDelegate != null)
						mUserScoresLoadDelegate.Invoke(scores.PopulationState == LeaderboardPopulationState.LPS_POPULATED ? scores : null);
				}
			}
		}

		public int GetAchievementCount()
		{
			if (mSessionActive)
				return (int)mAchievementManager.AchievementGroup.AchievementCount;
			else
				return -1;
		}

		public CoAchievement GetAchievementByID(long achievementID)
		{
			if (mSessionActive)
				return mAchievementManager.AchievementGroup.GetAchievementFromId(achievementID);
			else
				return null;
		}

		public CoAchievement GetAchievementByIndex(int index)
		{
			if (mSessionActive)
				return mAchievementManager.AchievementGroup.GetAchievementFromIndex((uint)index);
			else
				return null;
		}

		public bool IsAchievementUnlocked(long achievementID)
		{
			return mAchievementsUnlockedAtIndieCity.ContainsKey(achievementID);
		}

		public void UnlockAchievement(long achievementID)
		{
			mAchievementManager.UnlockAchievement(achievementID);
			mAchievementsUnlockedAtIndieCity[achievementID] = true;
		}

		public void RequestAchievementDataRefresh(ICCNotificationDelegate refreshCompleteDelegate)
		{
			if (mSessionActive) {
				if (mAchievementRefreshInfo != null)
					throw new Exception("Previous achievement refresh request still being processed");

				mAchievementRefreshInfo = new AchievementRefreshInfo(this, refreshCompleteDelegate, true);
			}
		}

		public void PostLeaderboardScore(int leaderboardID, long score)
		{
			mLeaderboardManager.GetLeaderboardFromId(leaderboardID).PostScore(score);
		}

		public CoLeaderboardPage RequestOpenLeaderboard(int leaderboardID, int pageSize, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
                if (mCurrentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");
                else
                {
                    mLeaderboardPageLoadDelegate = pageCompleteDelegate;
                    mCurrentlyLoadingLeaderboardPage = mLeaderboardManager.GetLeaderboardFromId(leaderboardID).GetGlobalPage((uint)pageSize);
                    return mCurrentlyLoadingLeaderboardPage;
                }
			}

				return null;
		}

		public void RequestFirstPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null)
					throw new Exception("Previous leaderboard page request still being processed");

				mLeaderboardPageLoadDelegate = pageCompleteDelegate;
				mCurrentlyLoadingLeaderboardPage = page;
				page.RequestFirst();
			}
		}

		public void RequestLastPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null)
					throw new Exception("Previous leaderboard page request still being processed");

				mLeaderboardPageLoadDelegate = pageCompleteDelegate;
				mCurrentlyLoadingLeaderboardPage = page;
				page.RequestLast();
			}
		}

		public void RequestPreviousPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null)
					throw new Exception("Previous leaderboard page request still being processed");

				mLeaderboardPageLoadDelegate = pageCompleteDelegate;
				mCurrentlyLoadingLeaderboardPage = page;
				page.RequestPrev();
			}
		}

		public void RequestNextPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null)
					throw new Exception("Previous leaderboard page request still being processed");

				mLeaderboardPageLoadDelegate = pageCompleteDelegate;
				mCurrentlyLoadingLeaderboardPage = page;
				page.RequestNext();
			}
		}

		public void RequestUserPage(CoLeaderboardPage page, ICCLeaderboardPageLoadDelegate pageCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingLeaderboardPage != null)
					throw new Exception("Previous leaderboard page request still being processed");

				mLeaderboardPageLoadDelegate = pageCompleteDelegate;
				mCurrentlyLoadingLeaderboardPage = page;
				page.RequestUserPage(mSession.UserId);
			}
		}

		public CoLeaderboardUserRows RequestUsersScores(ICCUserScoresLoadDelegate scoresCompleteDelegate)
		{
			if (mSessionActive) {
				if (mCurrentlyLoadingUserScores != null)
					throw new Exception("Previous user scores request still being processed");

				mUserScoresLoadDelegate = scoresCompleteDelegate;
				mCurrentlyLoadingUserScores = mLeaderboardManager.GetUsersScores(mSession.UserId);
				return mCurrentlyLoadingUserScores;
			}
			else
				return null;
		}

		public void HandleEvent(uint eventId, uint eventType, Array args)
		{
			if ((GameSessionEventCategory)eventType == GameSessionEventCategory.GSC_GameSession) {
				if ((GameSessionEvent)eventId == GameSessionEvent.GSE_SessionStarted) {
					CoAccessControl accessControl = new CoAccessControl();
					accessControl.Initialise(mSession);
					if (accessControl.LicenseState == LicenseState.LS_NOLICENSE) {
						mSessionEndBecauseNoLicense = true;
						mSession.EndSession();
					}
					else if (!mAchievementsReady || !mLeaderboardsReady) {
						mSessionStartPhase = SessionStartPhase.WAITING_FOR_INDIECITY_DATA;
						if (!mAchievementsReady)
							mAchievementRefreshInfo = new AchievementRefreshInfo(this, null, false);
					}
					else
						sessionActiveStateReached();
				}
				else if ((GameSessionEvent)eventId == GameSessionEvent.GSE_SessionEnded) {
					Enabled = false;
					mSessionActive = false;
					if (mSessionEndDelegate != null) {
						SessionEndReason reason;
						if (mSessionEndBecauseNoLicense)
							reason = SessionEndReason.NO_LICENSE;
						else {
							switch ((GameSessionReasonCode)(uint)((Object[])args)[0]) {
								default:
								case GameSessionReasonCode.GSR_Unknown:
									reason = SessionEndReason.UNKNOWN;
									break;
								case GameSessionReasonCode.GSR_UserRequest:
									reason = SessionEndReason.USER_REQUEST;
									break;
								case GameSessionReasonCode.GSR_NoConnection:
									reason = SessionEndReason.NO_CONNECTION;
									break;
								case GameSessionReasonCode.GSR_BadCredentials:
									reason = SessionEndReason.BAD_CREDENTIALS;
									break;
								case GameSessionReasonCode.GSR_TimeOut:
									reason = SessionEndReason.TIMEOUT;
									break;
							}
						}
						mSessionEndDelegate.Invoke(reason);
					}
					mSessionEndBecauseNoLicense = false;
				}
			}
		}

		public void OnLeaderboardsInitialised(bool modificationsDetected)
		{
			mLeaderboardsReady = true;
			if (mAchievementsReady)
				sessionActiveStateReached();
		}

		public void OnRowsDelivered(RowRequestContext context, Array rows)
		{
		}

		public void OnAchievementGroupInitialised(CoAchievementGroup pGroup, bool modificationsDetected)
		{
			OnAllAchievementsUpdated();
		}

		public void OnAchievementUnlocked(int UserId, CoAchievement pAchievement)
		{
		}

		public void OnAchievementUpdated(long AchievementId, uint TrueValue, uint AwardCount)
		{
		}

		public void OnAllAchievementsUpdated()
		{
			if (mAchievementRefreshInfo != null && !mAchievementRefreshInfo.mAchievementsUpdated) {
				mAchievementRefreshInfo.mAchievementsUpdated = true;
				checkIfAchievementDataComplete();
			}
		}

		public void OnUserAchievementsFetched(int UserId, Array args)
		{
			if (mAchievementRefreshInfo != null && !mAchievementRefreshInfo.mUserAchievementsFetched) {
				mAchievementsUnlockedAtIndieCity.Clear();
				if (args != null) {
					Object[] achievements = (Object[])args;
					for (int i = 0; i < achievements.Length; i++)
						mAchievementsUnlockedAtIndieCity[((CoAchievement)achievements[i]).AchievementId] = true;
				}
				mAchievementRefreshInfo.mUserAchievementsFetched = true;
				checkIfAchievementDataComplete();
			}
		}

		private void checkIfAchievementDataComplete()
		{
			if (mAchievementRefreshInfo.mAchievementsUpdated && mAchievementRefreshInfo.mUserAchievementsFetched) {
				try {
					if (mSessionStartPhase == SessionStartPhase.WAITING_FOR_INDIECITY_DATA) {
						mAchievementsReady = true;
						if (mLeaderboardsReady)
							sessionActiveStateReached();
					}
					else if (mAchievementRefreshInfo.mRefreshCompleteDelegate != null)
						mAchievementRefreshInfo.mRefreshCompleteDelegate.Invoke();
				}
				finally {
					mAchievementRefreshInfo = null;
				}
			}
		}

		private void sessionActiveStateReached()
		{
			mSessionActive = true;
			mSessionStartPhase = SessionStartPhase.ACTIVE;
			if (mSessionStartedDelegate != null)
				mSessionStartedDelegate.Invoke();
		}


		private class AchievementRefreshInfo
		{
			public bool mAchievementsUpdated;
			public bool mUserAchievementsFetched;
			public ICCNotificationDelegate mRefreshCompleteDelegate;

			public AchievementRefreshInfo(IndieCityComponent ic, ICCNotificationDelegate refreshCompleteDelegate, Boolean mustRefreshAchievementValues)
			{
				mAchievementsUpdated = false;
				mUserAchievementsFetched = false;
				mRefreshCompleteDelegate = refreshCompleteDelegate;

				ic.mAchievementManager.GetUserAchievementList(ic.mSession.UserId);
				if (mustRefreshAchievementValues)
					ic.mAchievementManager.AchievementGroup.RefreshAchievementValues();
			}
		}
	}
}
*/