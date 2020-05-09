using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WhenTestingCheckpointProgressTracker
    {

        private static int ZERO_LAPS_ELAPSED = -1;
        private static int ZERO_LAPS_ELAPSED_PASSED_CHECKER_FLAG = 0;
        private static int ONE_LAPS_ELAPSED = 0;
        private static int TWO_LAPS_ELAPSED = 0;

        private static int TARGET_CHECKPOINT_TREE = 3;
        private static int TARGET_CHECKPOINT_TWO = 2;
        private static int TOTAL_CHECKPOINTS = 15;
        private static float NEAR_TARGET_CHECKPOINT = 1f;
        private static float FAR_AWAY_FROM_TARGET_CHECKPOINT = 5f;

        private static float FIRST_POSITION_DISTANCE_TO_CHECKPOINT = 25.66f;
        private static float SECOND_POSITION_DISTANCE_TO_CHECKPOINT = 2.44f;
        private static float THIRD_POSITION_DISTANCE_TO_CHECKPOINT = 6.7f;
        

        [Test]
        public void ShouldBeBehindOpponentOnFirstLap() {

            var opponentDriver = new RaceStats(
                ZERO_LAPS_ELAPSED,
                TARGET_CHECKPOINT_TREE,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT );

            var testDriver = new RaceStats(
                ZERO_LAPS_ELAPSED,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT );

            bool shouldBeTrue = RaceHelperFuntions.IsOpponentInFront(opponentDriver, testDriver);
            Assert.IsTrue(shouldBeTrue);
        }

        [Test]
        public void ShouldBeInfrontOfOpponentOnFirstLap()
        {

            var opponentDriver = new RaceStats(
                ZERO_LAPS_ELAPSED,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT);

            var testDriver = new RaceStats(
                ZERO_LAPS_ELAPSED,
                TARGET_CHECKPOINT_TREE,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT);

            bool shouldBeFalse = RaceHelperFuntions.IsOpponentInFront(opponentDriver, testDriver);
            Assert.IsFalse(shouldBeFalse);
        }

        [Test]
        public void ShouldBeBehindOpponentWhenOpponentHasPassedCheckerPriorToFirstLapAndDriverHasNot()
        {
            var opponentDriver = new RaceStats(
                ZERO_LAPS_ELAPSED_PASSED_CHECKER_FLAG,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT);

            var testDriver = new RaceStats(
                ZERO_LAPS_ELAPSED,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                NEAR_TARGET_CHECKPOINT);

            bool shouldBeTrue = RaceHelperFuntions.IsOpponentInFront(opponentDriver, testDriver);
            Assert.IsTrue(shouldBeTrue);
        }

        [Test]
        public void ShouldInCorrectOrder()
        {
            var inFirstPositionDriver = new RaceStats(
                ZERO_LAPS_ELAPSED_PASSED_CHECKER_FLAG,
                TARGET_CHECKPOINT_TREE,
                TOTAL_CHECKPOINTS,
                FIRST_POSITION_DISTANCE_TO_CHECKPOINT);

            var inSecondPositionDriver = new RaceStats(
                ZERO_LAPS_ELAPSED_PASSED_CHECKER_FLAG,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                SECOND_POSITION_DISTANCE_TO_CHECKPOINT);

            var inThirdPositionDriver = new RaceStats(
                ZERO_LAPS_ELAPSED_PASSED_CHECKER_FLAG,
                TARGET_CHECKPOINT_TWO,
                TOTAL_CHECKPOINTS,
                THIRD_POSITION_DISTANCE_TO_CHECKPOINT);



            bool firstShouldBeInFrontOfSecond = RaceHelperFuntions.IsOpponentInFront(inFirstPositionDriver, inSecondPositionDriver);
            bool firstShouldBeInFrontOfThird = RaceHelperFuntions.IsOpponentInFront(inFirstPositionDriver, inThirdPositionDriver);

            bool secondShouldBeInFrontOfThrid = RaceHelperFuntions.IsOpponentInFront(inSecondPositionDriver, inThirdPositionDriver);

            bool thirdShouldBeBehindFirst = RaceHelperFuntions.IsOpponentInFront(inFirstPositionDriver, inThirdPositionDriver);
            bool thirdShouldBeBehindSecond = RaceHelperFuntions.IsOpponentInFront(inSecondPositionDriver, inThirdPositionDriver);

            Assert.IsTrue(firstShouldBeInFrontOfSecond);
            Assert.IsTrue(firstShouldBeInFrontOfThird);
            Assert.IsTrue(secondShouldBeInFrontOfThrid);

            Assert.IsTrue(thirdShouldBeBehindFirst);
            Assert.IsTrue(thirdShouldBeBehindSecond);
        }

    }
}
