{-# LANGUAGE TemplateHaskell, FlexibleInstances #-}
module Main where

import Graphics.Gloss
import Graphics.Gloss.Interface.IO.Interact
import Control.Lens


-- Common usage functions

dup :: a -> (a, a)
dup x = (x, x)

ext :: Either a a -> a
ext (Left x) = x
ext (Right x) = x


instance (Num a, Num b) => Num (a, b) where
    (x1, y1) + (x2, y2) = (x1 + x2, y1 + y2)
    (x1, y1) - (x2, y2) = (x1 - x2, y1 - y2)
    (x1, y1) * (x2, y2) = (x1 * x2, y1 * y2)
    abs = bimap abs abs
    signum = bimap signum signum
    fromInteger = bimap fromInteger fromInteger . dup


intersects :: Point -> Point -> Point -> Point -> Bool
intersects (x1, y1) (w1, h1) (x2, y2) (w2, h2) =
    abs (x1 - x2) < (w1 + w2) / 2 &&
    abs (y1 - y2) < (h1 + h2) / 2


data GameState = GameState {
    _velPlayer1 :: Point,
    _velPlayer2 :: Point,
    _posPlayer1 :: Point,
    _posPlayer2 :: Point,
    _velBall :: Point,
    _posBall :: Point,
    _scorePlayer1 :: Int,
    _scorePlayer2 :: Int
}

makeLenses ''GameState



-- Constants

windowSize2 :: Point
windowSize2 = (400, 300)

playerSize2 :: Point
playerSize2 = (15, 75)

playerSize :: Point
playerSize = 2 * playerSize2

ballSize2 :: Point
ballSize2 = dup $ fst playerSize2

ballSize :: Point
ballSize = 2 * ballSize2

gapPlayers :: Float
gapPlayers = 35 + fst playerSize2

velPlayer :: Float
velPlayer = 300

velBall' :: Float 
velBall' = 1.5 * velPlayer



render :: GameState -> Picture 
render gs = 
    (
        uncurry rectangleSolid playerSize &
        Color white &
        uncurry Translate (gs ^. posPlayer1)
    ) <>
    (
        uncurry rectangleSolid playerSize &
        Color white &
        uncurry Translate (gs ^. posPlayer2)
    ) <>
    (
        uncurry rectangleSolid ballSize &
        Color (greyN 0.5) &
        uncurry Translate (gs ^. posBall)
    ) <>
    (
        Text (show (gs ^. scorePlayer1) ++ " / " ++
            show (gs ^. scorePlayer2)) &
        Color red &
        Scale 0.2 0.2 &
        Translate (-50) (snd windowSize2 - 50)
    )


step :: Float -> GameState -> GameState
step dt = updateScores . checkCollisions . updateDistance
    where
        updateDistance gs = gs &~ do
            posPlayer1 += dup dt * gs ^. velPlayer1
            posPlayer2 += dup dt * gs ^. velPlayer2
            posBall += dup dt * gs ^. velBall

        checkCollisions gs = gs &~ do
            velBall . _2 *=
                if abs (gs ^. posBall . _2) + snd ballSize2 >= snd windowSize2
                then -1 else 1
            velBall . _1 *= if bounce1 || bounce2 then -1 else 1
            posBall . _1 .=
                if bounce1 then
                    gs ^. posPlayer1 . _1 + fst ballSize2 + fst playerSize2
                else if bounce2 then
                    gs ^. posPlayer2 . _1 - fst ballSize2 - fst playerSize2
                else gs ^. posBall . _1
            posPlayer1 . _2 .=
                if gs ^. posPlayer1 . _2 + snd playerSize2 >
                    snd windowSize2
                then snd windowSize2 - snd playerSize2
                else if gs ^. posPlayer1 . _2 - snd playerSize2 <
                    -snd windowSize2
                then snd playerSize2 - snd windowSize2
                else gs ^. posPlayer1 . _2
            posPlayer2 . _2 .=
                if gs ^. posPlayer2 . _2 + snd playerSize2 >
                    snd windowSize2
                then snd windowSize2 - snd playerSize2
                else if gs ^. posPlayer2 . _2 - snd playerSize2 <
                    -snd windowSize2
                then snd playerSize2 - snd windowSize2
                else gs ^. posPlayer2 . _2
            where
                bounce1 = intersects (gs ^. posBall) ballSize
                    (gs ^. posPlayer1) playerSize
                bounce2 = intersects (gs ^. posBall) ballSize
                    (gs ^. posPlayer2) playerSize

        updateScores gs = gs &~ do
            posBall .= if score1 || score2 then dup 0 else gs ^. posBall
            scorePlayer1 %= if score1 then succ else id
            scorePlayer2 %= if score2 then succ else id
            where
                score1 = gs ^. posBall . _1 + fst ballSize2 >=
                    fst windowSize2
                score2 = -(gs ^. posBall . _1 + fst ballSize2) >=
                    fst windowSize2


events :: Event -> GameState -> GameState
events evt gs = case evt of
    EventKey (Char 'w') e _ _ -> gs & velPlayer1 . _2 .~ speed e
    EventKey (Char 's') e _ _ -> gs & velPlayer1 . _2 .~ -speed e
    EventKey (SpecialKey KeyUp) e _ _ -> gs & velPlayer2 . _2 .~ speed e
    EventKey (SpecialKey KeyDown) e _ _ -> gs & velPlayer2 . _2 .~ -speed e
    _ -> gs
    where
        speed Down = velPlayer
        speed Up = 0


main :: IO ()
main = play
    (InWindow "Pong" (bimap floor floor (2 * windowSize2)) (300, 80))
    black
    60
    (GameState {
        _velPlayer1 = (0, 0),
        _velPlayer2 = (0, 0),
        _posPlayer1 = (negate (fst windowSize2) + gapPlayers, 0),
        _posPlayer2 = (fst windowSize2 - gapPlayers, 0),
        _velBall = dup velBall',
        _posBall = dup 0,
        _scorePlayer1 = 0,
        _scorePlayer2 = 0
    })
    render
    events
    step
