using System;
using System.Collections.Generic;

namespace Tiandao.LBS
{
    public class Position
    {
		#region 公共属性

		public Location LeftTop
	    {
		    get;
		    private set;
	    }

	    public Location LeftBottom
	    {
		    get;
		    private set;
	    }

	    public Location RightTop
	    {
		    get;
		    private set;
	    }

	    public Location RightBottom
	    {
		    get;
		    private set;
	    }

		#endregion

		#region 构造方法

	    public Position(Location leftTop, Location leftBottom, Location rightTop, Location rightBottom)
	    {
			if(leftTop == null)
				throw new ArgumentNullException("leftTop");

			if(leftBottom == null)
				throw new ArgumentNullException("leftBottom");

			if(rightTop == null)
				throw new ArgumentNullException("rightTop");

			if(rightBottom == null)
				throw new ArgumentNullException("rightBottom");

		    this.LeftTop = leftTop;
		    this.LeftBottom = leftBottom;
		    this.RightTop = rightTop;
		    this.RightBottom = rightBottom;
	    }

	    #endregion
	}
}
