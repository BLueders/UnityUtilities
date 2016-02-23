using UnityEngine;
using System.Collections;
using System;

namespace Toolbox {
	[System.Serializable]
	public struct IntVector3 {
		public IntVector3(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public enum Mode {Cast, Floor, Ceil, Round};
		public IntVector3(Vector3 vector3, Mode mode) {
			switch (mode) {
				case Mode.Cast:
					this.x = (int)vector3.x;
					this.y = (int)vector3.y;
					this.z = (int)vector3.z;
					break;

				case Mode.Floor:
					this.x = Mathf.FloorToInt(vector3.x);
					this.y = Mathf.FloorToInt(vector3.y);
					this.z = Mathf.FloorToInt(vector3.z);
					break;

				case Mode.Ceil:
					this.x = Mathf.CeilToInt(vector3.x);
					this.y = Mathf.CeilToInt(vector3.y);
					this.z = Mathf.CeilToInt(vector3.z);
					break;

				case Mode.Round:
					this.x = Mathf.RoundToInt(vector3.x);
					this.y = Mathf.RoundToInt(vector3.y);
					this.z = Mathf.RoundToInt(vector3.z);
					break;

				default:
					this.x = 0;
					this.y = 0;
					this.z = 0;
					break;                  
			}
		}

		public int x;
		public int y;
		public int z;

		public static implicit operator Vector3(IntVector3 v) {
			return new Vector3(v.x, v.y, v.z);
		}

		//Operators
		public override string ToString() {
			return "IntVector3 (" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
		}

		public static IntVector3 operator +(IntVector3 v0, IntVector3 v1) {
			return new IntVector3(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
		}

		public static bool operator ==(IntVector3 v0, IntVector3 v1) {
			return (v0.x == v1.x) && (v0.y == v1.y) && (v0.z == v1.z);
		}

		public static bool operator !=(IntVector3 v0, IntVector3 v1) {
			return (v0.x != v1.x) || (v0.y != v1.y) | (v0.z != v1.z);
		}

		public static IntVector3 operator -(IntVector3 v0, IntVector3 v1) {
			return new IntVector3(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
		}

		public override bool Equals(object obj) {
			if (obj is IntVector3) {
				IntVector3 v = (IntVector3)obj;
				return (x == v.x) && (y == v.y) && (z == v.z);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			int result = x;
			unchecked {
				result = (result * 397) ^ y;
				result = (result * 397) ^ z;
			}
			return result;
		}

		//Default vectors
		public static IntVector3 zero {
			get { return new IntVector3(0, 0, 0); }
		}

		public static IntVector3 right {
			get { return new IntVector3(1, 0, 0); }
		}

		public static IntVector3 left {
			get { return new IntVector3(-1, 0, 0); }
		}

		public static IntVector3 up {
			get { return new IntVector3(0, 1, 0); }
		}

		public static IntVector3 down {
			get { return new IntVector3(0, -1, 0); }
		}

		public static IntVector3 forward {
			get { return new IntVector3(0, 0, 1); }
		}

		public static IntVector3 back {
			get { return new IntVector3(0, 0, -1); }
		}
	}
}
