using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathDemo
{
    public class OBB2D
    {
        public Vector2 origin;
        public Vector2 size;
        Vector2 LocalXAxis;
        Vector2 LocalYAxis;

        float angle;
        public float AxisAngle
        {
            get { return angle; }
            set
            {
                angle = MathHelper.WrapAngle(value);
                LocalXAxis = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                LocalYAxis = new Vector2(-(float)Math.Sin(angle), (float)Math.Cos(angle));
            }
        }

        public OBB2D(Vector2 origin, float angle, Vector2 boxSize)
        {
            this.origin = origin;
            angle = MathHelper.WrapAngle(angle);
            this.angle = angle;
            LocalXAxis = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            LocalYAxis = new Vector2(-(float)Math.Sin(angle), (float)Math.Cos(angle));
            size = boxSize;
        }

        public void Draw(SpriteBatch batch, Texture2D boxTexture)
        {
            batch.Draw(boxTexture, new Rectangle((int)origin.X, (int)origin.Y, (int)size.X, (int)size.Y),
                null, Color.White, angle, Vector2.One / 2, SpriteEffects.None, 0);
        }

        public bool Intersects(OBB2D targetBox)
        {
            float projectedSource, projectedTarget;         // source and target projection radii onto selected axes
            float[,] Rot = new float[2, 2];                 // scalar values representing rotation onto local axes
            float[,] AbsRot = new float[2, 2];              // absolute values of those scalars

            // get the distance between the two box origins
            Vector2 distanceOnLocalAxes = targetBox.origin - origin;

            // then rotate it so that it's expressed in the local box axes
            distanceOnLocalAxes = new Vector2(Vector2.Dot(distanceOnLocalAxes, LocalXAxis),
                Vector2.Dot(distanceOnLocalAxes, LocalYAxis));

            // calculate rotated proportions that will place boxes in correct frame of reference
            Rot[0, 0] = Vector2.Dot(LocalXAxis, targetBox.LocalXAxis);
            Rot[0, 1] = Vector2.Dot(LocalXAxis, targetBox.LocalYAxis);
            Rot[1, 0] = Vector2.Dot(LocalYAxis, targetBox.LocalXAxis);
            Rot[1, 1] = Vector2.Dot(LocalYAxis, targetBox.LocalYAxis);

            // add a tiny bit on to correct for potential rounding errors
            // in floating point calculations
            AbsRot[0, 0] = Math.Abs(Rot[0, 0]) + 0.00001f;  
            AbsRot[0, 1] = Math.Abs(Rot[0, 1]) + 0.00001f;
            AbsRot[1, 0] = Math.Abs(Rot[1, 0]) + 0.00001f;
            AbsRot[1, 1] = Math.Abs(Rot[1, 1]) + 0.00001f;

            // Check if the local X axis is a separating axis.
            // First, get the size of one half of the source box on its own axis.
            projectedSource = size.X / 2;
            // Then, get the size of one half of the target box on that axis, 
            // using its own size and those rotated proportions we worked out earlier
            projectedTarget = (targetBox.size.X / 2) * AbsRot[0, 0] 
                + (targetBox.size.Y / 2) * AbsRot[0, 1];

            // Then, if the size of the two projected half-lengths is less than the distance 
            // between the origins then we have found an axis that separates them so there
            // is no intersection.
            if (Math.Abs(distanceOnLocalAxes.X) > (projectedSource + projectedTarget))
                return false;

            // Next, check the local Y axis for a separating axis
            projectedSource = size.Y / 2;            
            projectedTarget = (targetBox.size.X / 2) * AbsRot[1, 0] + 
                (targetBox.size.Y / 2) * AbsRot[1, 1];
            if (Math.Abs(distanceOnLocalAxes.Y) > (projectedSource + projectedTarget))
                return false;

            // Now check the target's X axis
            projectedSource = (size.X / 2) * AbsRot[0, 0] + (size.Y / 2) * AbsRot[1, 0];    
            projectedTarget = targetBox.size.X / 2;
            if (Math.Abs(distanceOnLocalAxes.X * Rot[0, 0] + distanceOnLocalAxes.Y * Rot[1, 0]) > (projectedSource + projectedTarget))
                return false;

            // Now check the target's Y axis
            projectedSource = (size.X / 2) * AbsRot[0, 1] + (size.Y / 2) * AbsRot[1, 1];
            projectedTarget = targetBox.size.Y / 2;
            if (Math.Abs(distanceOnLocalAxes.X * Rot[0, 1] + distanceOnLocalAxes.Y * Rot[1, 1]) > (projectedSource + projectedTarget))
                return false;

            // no separating axis found - therefore the two boxes intersect.
            return true;    
        }

        public bool Intersects(BoundingBox box)
        {
            Vector3 origin3 = box.Min + (box.Max - box.Min) / 2;
            Vector2 targetorigin = new Vector2(origin3.X, origin3.Y);
            Vector2 size = new Vector2(box.Max.X - box.Min.X, box.Max.Y - box.Min.Y);
            OBB2D o = new OBB2D(targetorigin, 0, size);
            return Intersects(o);
        }

        public bool Intersects(Rectangle rectangle)
        {
            Vector2 targetorigin = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            Vector2 size = new Vector2(rectangle.Width, rectangle.Height);
            OBB2D o = new OBB2D(targetorigin, 0, size);
            return Intersects(o);
        }
    }

}
