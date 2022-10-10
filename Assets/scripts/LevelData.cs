using UnityEngine;
using System.Collections;

public class LevelData {
	// note, the level is bounded by (+-4.5, +-4.5)
	// these coordinates are 5 game coordinates
	// so 4.2 = 21 is roughly the max
	public const string data = @"{
player: {x:0, y:0},
waves: [
	{
		enemies: [
			{type:0, x:0, y:20},
		],
		duration: 4
	},
	{
		enemies: [
			{type:0, x:-2, y:20},
			{type:0, x:2, y:20},
		],
		duration: 4
	},
	{
		enemies: [
			{type:1, x:20, y:20},
		],
		duration: 8
	},
	{
		enemies: [
			{type:1, x:-20, y:20},
			{type:0, x:8, y:-20},
		],
		duration: 8
	},

	{
		enemies: [
			{type:0, x:17, y:6},
			{type:0, x:17, y:2},
		],
		duration: 0.2
	},
	{
		enemies: [
			{type:0, x:18, y:8},
			{type:0, x:18, y:3},
		],
		duration: 0.2
	},
	{
		enemies: [
			{type:0, x:19, y:10},
			{type:0, x:19, y:4},
		],
		duration: 6
	},

	{
		enemies: [
			{type:2, x:-20, y:20},
			{type:2, x:20, y:-20},
		],
		duration: 6
	},
	{
		enemies: [
			{type:2, x:0, y:20},
			{type:1, x:5, y:-20},
			{type:1, x:-5, y:-20},
		],
		duration: 6
	},

	{
		wave_iter: {
			n: 12,
			a: 30,
			r: 0.3,
			delay: 0.3,
		},
		enemies: [
			{type:0, r:15, a:0, polar:true},
		],
		duration: 8
	},

	{
		wave_iter: {
			n: 2,
			delay: 10,
			a: 180,
		}
		enemy_iter: {
			n: 9,
			a: 30,
			r: 0.3,
		},
		enemies: [
			{type:0, r:15, a:-30, polar:true},
		],
		duration: 10
	},
	{
		enemy_iter: {
			n: 3,
			x: 16,
		},
		enemies: [
			{type:1, x:-16, y:20},
		],
		duration: 12
	},
	{
		enemy_iter: {
			n: 5,
			a: 22,
		},
		enemies: [
			{type:0, a:135, r:19, polar:true},
		],
		duration: 1
	},
	{
		enemies: [
			{type:2, x:0, y:-20},
		],
		duration: 2
	},
	{
		enemies: [
			{type:0, x:-20, y:20},
			{type:0, x:20, y:20},
		],
		duration: 10
	},
	{
		enemies: [
			{type:1, x:-20, y:20},
			{type:2, x:20, y:20},
			{type:1, x:20, y:-20},
			{type:2, x:-20, y:-20},
		],
		duration: 20
	},
	{
		enemy_iter: {
			n: 3,
			a: 45,
		},
		enemies: [
			{type:0, a:45, r:19, polar:true},
			{type:1, a:-135, r:19, polar:true},
		],
		duration: 10
	},

	{
		wave_iter: {
			n: 2,
			delay: 10,
			a: 120,
		}
		enemy_iter: {
			n: 9,
			a: 30,
			r: 0.3,
		},
		enemies: [
			{type:0, r:15, a:-30, polar:true},
		],
		duration: 2
	},

	{
		enemies: [
			{type:2, x:-20, y:20},
			{type:2, x:20, y:-20},
		],
		duration: 6
	},
	{
		enemy_iter: {
			n: 4,
			a: 90,
			r: -1,
		},
		enemies: [
			{type:0, r:18, a:-30, polar:true},
		],
		duration: 2
	},
]
,testWave: 0
}";
}
