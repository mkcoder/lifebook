﻿using System;
using System.Linq;
using lifebook.core.processmanager.Domain;

namespace lifebook.core.processmanager.Models
{
    public class DefaultProcessNameAssigner
    {
        private static string random_words =       
@"manual
precede
cancel
theft
earwax
detective
alarm
knot
nest
senior
holiday
rehabilitation
heel
brown
attraction
will
exact
block
twitch
feminine
participate
motorist
jump
belief
judge
capital
poor
dance
glimpse
oil
round
egg
trial
dedicate
zone
change
jockey
punish
pair
exposure
television
sniff
enthusiasm
pit
row
latest
liberty
leader
predator
strong
diameter
document
garlic
revolution
leaf
celebration
decoration
shiver
revise
snarl
prospect
memorial
opponent
minor
factor
college
top
bean
tiptoe
catalogue
account
notion
policeman
canvas
tablet
youth
freeze
tumble
loud
strap
flat
cotton
guard
scrape
texture
kid
present
opposite
penetrate
mug
despair
virgin
operational
keep
lid
dish
deer
conference
basket
worry";

        public static string GetName()
        {
            return string.Join("-", random_words.Split("\n").OrderBy(_ => Guid.NewGuid()).Take(2));
        }
    }
}
