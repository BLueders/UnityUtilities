using UnityEngine;
using System.Collections;
using System;

namespace Toolbox {
	[System.Serializable]
	public struct IntVector2 {
		public IntVector2(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public enum Mode {Cast, Floor, Ceil, Round};
		public IntVector2(Vector2 vector2, Mode mode) {
			switch (mode) {
				case Mode.Cast:
					x = (int)vector2.x;
					y = (int)vector2.y;
					break;

				case Mode.Floor:
					x = Mathf.FloorToInt(vector2.x);
					y = Mathf.FloorToInt(vector2.y);
					break;

				case Mode.Ceil:
					x = Mathf.CeilToInt(vector2.x);
					y = Mathf.CeilToInt(vector2.y);
					break;

				case Mode.Round:
					x = Mathf.RoundToInt(vector2.x);
					y = Mathf.RoundToInt(vector2.y);
					break;

				default:
					x = 0;
					y = 0;
					break;                  
			}
		}

		public int x;
		public int y;

		public static implicit operator Vector2(IntVector2 v) {
			return new Vector2(v.x, v.y);
		}

		//Operators
		public override string ToString() {
			return "IntVector2 (" + x.ToString() + ", " + y.ToString() + ")";
		}

		public static IntVector2 operator +(IntVector2 v0, IntVector2 v1) {
			return new IntVector2(v0.x + v1.x, v0.y + v1.y);
		}

		public static bool operator ==(IntVector2 v0, IntVector2 v1) {
			return (v0.x == v1.x) && (v0.y == v1.y);
		}

		public static bool operator !=(IntVector2 v0, IntVector2 v1) {
			return (v0.x != v1.x) || (v0.y != v1.y);
		}

		public static IntVector2 operator -(IntVector2 v0, IntVector2 v1) {
			return new IntVector2(v0.x - v1.x, v0.y - v1.y);
		}

		public override bool Equals(object obj) {
			if (obj is IntVector2) {
				IntVector2 v = (IntVector2)obj;
				return (x == v.x) && (y == v.y);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			int result = x;
			unchecked {
				result = (result * 397) ^ y;
			}
			return result;
		}

		//Default vectors
		public static IntVector2 zero {
			get { return new IntVector2(0, 0); }
		}

		public static IntVector2 right {
			get { return new IntVector2(1, 0); }
		}

		public static IntVector2 left {
			get { return new IntVector2(-1, 0); }
		}

		public static IntVector2 up {
			get { return new IntVector2(0, 1); }
		}

		public static IntVector2 down {
			get { return new IntVector2(0, -1); }
		}
	}
}
